using System;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Player : Character, IFlashable
{
    /// Number of lives the Player has
    internal const int NumLives = 9;

    [Header("Movement Parameters")] 
    [SerializeField] private float _jumpForce;

    [SerializeField] private float _topSpeed;
    [SerializeField] private float _acceleration = 4f;

    [SerializeField] private float _deceleration = 10f; // Bigger value = harder stop

    /// How much to change the gravity when Player is falling
    [Range(1, 5)]
    [SerializeField] private float _fallGravityMultiplier;

    [Header("Flash Animation Parameters")]
    [SerializeField] private int _flashCount = 5;
    [SerializeField] private float _flickDuration = 0.1f;
    [SerializeField] private Color _flashColor;
    private bool _isInvincible;

    /// "Normal" gravity when not jumping
    private float _legacyGravityScale;

    /// Player's SpriteRenderer component
    private SpriteRenderer _sr;

    /// Cat skins
    [SerializeField] 
    private SpriteLibraryAsset[] _sprites = new SpriteLibraryAsset[5];

    private void OnValidate()
    {
        if (_sprites.Length == 5) return;
        
        Debug.LogWarning("\"Sprites\" array must be of length 5");
        Array.Resize(ref this._sprites, 5);

    }
    
    private void OnEnable()
    {
        EventManager.Events.OnFoxHitsPlayer += HandleFoxHitsPlayer;
    }
    
    private void OnDisable()
    {
        EventManager.Events.OnFoxHitsPlayer -= HandleFoxHitsPlayer;
    }

    protected override void Awake()
    {
        base.Awake();
        _sr = GetComponent<SpriteRenderer>();
        _legacyGravityScale = rb.gravityScale;

        SpriteLibrary sl = GetComponent<SpriteLibrary>();
        sl.spriteLibraryAsset = _sprites[SaveSystem.LoadPreferences().CatID - 1];
    }

    protected void Start()
    {
        // Put Player at screen 0
        Transform trans = transform;
        Vector3 position = trans.position;
        Vector3 newPosition = new Vector3(0, position.y, position.z);
        trans.position = newPosition;
    }

    protected void Update()
    {
        // Set animation parameters
        SetRunAnimationParam(IsVisiblyRunning);
        SetJumpAnimationParam(IsVisiblyJumping);
        SetFallAnimationParam(IsFalling());

        // Return if Player is in any motion
        if (IsRunning() || !this.IsGrounded || this.rb.velocity.x > 0.1f)
            return;

        SetSpeedAsIdle();
    }

    protected void FixedUpdate()
    {
        // Handle Run input
        int playerInputDirection = GetInputDirection();
        IsVisiblyRunning = playerInputDirection != 0;
        HandleRunInput(playerInputDirection);

        // Handle Jump input
        IsVisiblyJumping = IsJumping();

        // Conditionally Handle Fall
        if (IsFalling()) IncreaseFallGravity();
        else ResetGravity();
    }

    /// Returns if either the Right Arrow key or Left Arrow key are held
    protected override bool IsRunning()
    {
        return (Input.GetKey(KeyCode.RightArrow) ||
                Input.GetKey(KeyCode.LeftArrow));
    }

    /// Handles the acceleration and sprite direction after
    /// the player has input Run
    private void HandleRunInput(int inputDirection)
    {
        // Force-based movement
        float targetVelocity = inputDirection * _topSpeed;
        float speedDiff = targetVelocity - rb.velocity.x;
        float accelerationRate = (Mathf.Abs(targetVelocity) > 0.01f)
            ? _acceleration
            : _deceleration;
        float movement = Mathf.Abs(speedDiff) * accelerationRate *
                         Mathf.Sign(speedDiff);
        rb.AddForce(movement * Vector2.right);

        // Switch direction
        HandleFlipSprite();
    }

    /// Returns if the space-bar is pressed and the Player is grounded
    protected override bool IsJumping()
    {
        return (Input.GetKey(KeyCode.Space) && this.IsGrounded);
    }

    protected override void HandleJumpAnimationEvent()
    {
        // TODO - Player jump on specific keyframe
        rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    /// Strengthens the RigidBody2D fall gravity by a factor of `fallGravityMultiplier`
    private void IncreaseFallGravity()
    {
        rb.gravityScale = _legacyGravityScale * _fallGravityMultiplier;
    }

    /// Resets the RigidBody2D's gravity scale to its original value
    private void ResetGravity()
    {
        this.rb.gravityScale = this._legacyGravityScale;
    }


    /// Determines the direction of input from the user.
    private static int GetInputDirection()
    {
        if (Input.GetKey(KeyCode.RightArrow)) return 1;
        if (Input.GetKey(KeyCode.LeftArrow)) return -1;
        return 0;
    }

    /// Flips the Player's sprite depending on its direction
    private void HandleFlipSprite()
    {
        this._sr.flipX = GetInputDirection() switch
        {
            -1 => true,
            1 => false,
            _ => this._sr.flipX
        };
    }

    private void HandleFoxHitsPlayer()
    {
        // Disregard if Player is already invincible
        if (this._isInvincible) return;

        _isInvincible = true;
        EventManager.Events.PlayerInvincible();
        UIManager.ui.LoseHeart();
        StartCoroutine(FlashEffect());
        
    }

    public IEnumerator FlashEffect()
    {
        for (int i = 0; i < _flashCount; i++)
        {
            // Make the sprite red
            _sr.color = _flashColor;
            yield return new WaitForSeconds(this._flickDuration);

            // Make the sprite opaque
            // ReSharper disable once Unity.InefficientPropertyAccess
            _sr.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(this._flickDuration);
        }

        this._isInvincible = false;
        EventManager.Events.PlayerVulnerable();
    }
}