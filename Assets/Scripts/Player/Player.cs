using System;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation;

public sealed class Player : Character, IFlashable
{
    [Header("Movement Parameters")] 
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _topSpeed;
    [SerializeField] private float _acceleration = 4f;
    [SerializeField] private float _deceleration = 10f; // Bigger value = harder stop
    [SerializeField] private float _outOfBoundsForce = 20f;
    
    /// How much to change the gravity when Player is falling
    [Range(1, 5)]
    [SerializeField] private float _fallGravityMultiplier;

    [Header("Flash Animation Parameters")]
    [SerializeField] private int _flashCount = 5;
    [SerializeField] private float _flickDuration = 0.1f;
    [SerializeField] private Color _flashColor;
    
    private int _playerInputDirection;
    private float _legacyGravityScale;
    private bool _isInvincible;
    private bool _gameIsPaused;
    private bool _gameHasStarted;
    private static readonly float JumpTrailDisplacement = 20f;
    private static readonly float Partition = Screen.width / 2f;
    internal const int NumLives = 9;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    /// Cat skins
    [SerializeField] 
    private SpriteLibraryAsset[] _sprites = new SpriteLibraryAsset[5];

    private void OnValidate()
    {
        if (_sprites.Length == 5) return;
        
        Debug.LogWarning("\"Sprites\" array must be of length 5");
        Array.Resize(ref _sprites, 5);
    }
    
    private void OnEnable()
    {
        EventManager.Events.OnFoxHitsPlayer += HandleFoxHitsPlayer;
        EventManager.Events.OnPauseKeyDown += HandlePause;
        EventManager.Events.OnGameOver += HandleGameOver;
    }
    
    private void OnDisable()
    {
        EventManager.Events.OnFoxHitsPlayer -= HandleFoxHitsPlayer;
        EventManager.Events.OnPauseKeyDown -= HandlePause;
        EventManager.Events.OnGameOver -= HandleGameOver;
    }

    private void OnBecameInvisible()
    {
        // Set force direction to inward
        float direction = -Mathf.Sign(transform.position.x);
        Vector2 force = Vector2.right * _outOfBoundsForce * direction;
        _rb.AddForce(force, ForceMode2D.Impulse);
    }

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _legacyGravityScale = _rb.gravityScale;

        SpriteLibrary sl = GetComponent<SpriteLibrary>();
        sl.spriteLibraryAsset = _sprites[SaveSystem.LoadPreferences().CatID - 1];
    }

    private IEnumerator Start()
    {
        // Put Player at screen 0
        Transform trans = transform;
        Vector3 position = trans.position;
        Vector3 newPosition = new Vector3(0, position.y, position.z);
        trans.position = newPosition;

        yield return StartCoroutine(WaitForInput());
    }

    private void Update()
    {
        // Handle Run input
        _playerInputDirection = GetInputDirection();
        IsVisiblyRunning = _playerInputDirection != 0;
        
        // Handle Jump input
        IsVisiblyJumping = IsJumping();
        
        // Set animation parameters
        SetRunAnimationParam(IsVisiblyRunning);
        SetJumpAnimationParam(IsVisiblyJumping);
        SetFallAnimationParam(IsFalling());

        // Return if Player is in any motion
        if (!IsGrounded || _rb.velocity.x > 0.1f)
            return;

        if (!_gameHasStarted) return;
        SetSpeedAsIdle();
    }

    private void FixedUpdate()
    {
        HandleRunInput(_playerInputDirection);

        // Conditionally Handle Fall
        if (IsFalling()) IncreaseFallGravity();
        else ResetGravity();
    }

    /// Determines the direction of input from the user.
    private int GetInputDirection()
    {
        // Keyboard inputs
        if (Input.GetKey(KeyCode.RightArrow)) return 1;
        if (Input.GetKey(KeyCode.LeftArrow)) return -1;
        
        // Touch inputs
        foreach (var touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    return touch.position.x >= Partition ? 1 : -1;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        return 0;
    }

    /// Returns if the space-bar is pressed and the Player is grounded
    protected override bool IsJumping()
    {
        if (!IsGrounded) return false;
        
        // Keyboard Input
        if (Input.GetKey(KeyCode.Space)) return true;
        
        // Touch input
        foreach (var touch in Input.touches) if (touch.phase == TouchPhase.Moved)
        {
            // Jump if Player swipes up
            // TODO - Make more responsive using timing, dynamic distance
            return touch.deltaPosition.y <= - JumpTrailDisplacement;
        }
        
        return false;
    }

    /// Handles the acceleration and sprite direction after
    /// the player has input Run
    private void HandleRunInput(int inputDirection)
    {
        // Force-based movement
        float targetVelocity = inputDirection * _topSpeed;
        float speedDiff = targetVelocity - _rb.velocity.x;
        float accelerationRate = (Mathf.Abs(targetVelocity) > 0.01f)
            ? _acceleration
            : _deceleration;
        float movement = Mathf.Abs(speedDiff) * accelerationRate *
                         Mathf.Sign(speedDiff);
        _rb.AddForce(movement * Vector2.right);

        // Switch direction
        HandleFlipSprite();
    }

    /// Strengthens the RigidBody2D fall gravity by a factor of `fallGravityMultiplier`
    private void IncreaseFallGravity()
    {
        _rb.gravityScale = _legacyGravityScale * _fallGravityMultiplier;
    }

    /// Resets the RigidBody2D's gravity scale to its original value
    private void ResetGravity()
    {
        _rb.gravityScale = _legacyGravityScale;
    }

    /// <inheritdoc cref="Character.HandleJumpAnimationEvent"/>,
    /// and applies an upward impulse force of magnitude _jumpForce
    protected override void HandleJumpAnimationEvent()
    {
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    /// Triggers when the player collides with a fox
    private void HandleFoxHitsPlayer()
    {
        // Disregard if Player is already invincible
        if (_isInvincible) return;

        _isInvincible = true;
        EventManager.Events.PlayerInvincible();
        int numHearts = UIManager.ui.LoseHeart();
        
        // Don't flash if is last heart
        if (numHearts != 0) StartCoroutine(FlashEffect());
    }
    
    public IEnumerator FlashEffect()
    {
        for (int i = 0; i < _flashCount; i++)
        {
            // Make the sprite red
            _sr.color = _flashColor;
            yield return new WaitForSeconds(_flickDuration);

            // Make the sprite opaque
            // ReSharper disable once Unity.InefficientPropertyAccess
            _sr.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(_flickDuration);
        }

        _isInvincible = false;
        EventManager.Events.PlayerVulnerable();
    }

    /// Flips the Player's sprite depending on its direction
    private void HandleFlipSprite()
    {
        _sr.flipX = GetInputDirection() switch
        {
            -1 => true,
            1 => false,
            _ => _sr.flipX
        };
    }
    
    /// Waits for the player to input a direction before signaling
    /// to other components to begin processes to run the gameplay
    private IEnumerator WaitForInput()
    {
        // if (_gameIsPaused) yield return null;
        while (_gameIsPaused || (GetInputDirection() == 0 && !IsJumping()))
            yield return null;
        EventManager.Events.PlayStart();
        _gameHasStarted = true;
    }

    /// Used to prevent the game from starting via player input
    /// while the game is paused
    private void HandlePause(bool isPaused)
    {
        _gameIsPaused = isPaused;
    }
    
    /// Disables script on Game Over
    private void HandleGameOver()
    {
        SetDyingAnimationParam(true);
        
        // Set other animations to false to prevent flickering
        SetRunAnimationParam(false);
        SetJumpAnimationParam(false);
        SetFallAnimationParam(false);
        
        this.enabled = false;
        // TODO - Add bounce to material so Player bounces off floor when dead
    }

}