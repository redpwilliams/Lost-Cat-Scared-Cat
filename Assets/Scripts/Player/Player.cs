using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D.Animation;

public class Player : Character, IFlashable
{
    /// Initial starting point of player
    internal const float PlayerXPos = 0f;

    /// Number of lives the Player has
    internal const int NumLives = 9;

    [Header("Movement Parameters")] 
    [SerializeField] private float jumpForce;

    [SerializeField] private float topSpeed;
    [SerializeField] private float acceleration = 4f;

    [SerializeField] private float deceleration = 10f; // Bigger value = harder stop

    /// How much to change the gravity when Player is falling
    [Range(1, 5)]
    [SerializeField] private float fallGravityMultiplier;

    [Header("Flash Animation Parameters")]
    [SerializeField] private int flashCount = 5;
    [SerializeField] private float flickDuration = 0.1f;
    private bool isInvincible;

    /// "Normal" gravity when not jumping
    private float legacyGravityScale;

    /// Player's SpriteRenderer component
    private SpriteRenderer sr;

    /// Cat skins
    [SerializeField] 
    private SpriteLibraryAsset[] sprites = new SpriteLibraryAsset[5];

    private void OnValidate()
    {
        if (sprites.Length == 5) return;
        
        Debug.LogWarning("\"Sprites\" array must be of length 5");
        Array.Resize(ref this.sprites, 5);

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
        sr = GetComponent<SpriteRenderer>();
        legacyGravityScale = this.rb.gravityScale;

        SpriteLibrary sl = GetComponent<SpriteLibrary>();
        Debug.Log(sl.spriteLibraryAsset.name);
        sl.spriteLibraryAsset = sprites[SaveSystem.LoadPreferences().CatID - 1];
    }

    protected void Start()
    {
        // Put Player on specific location on screen
        Vector3 position = transform.position;
        Vector3 newPosition = new Vector3(PlayerXPos, position.y, position.z);
        gameObject.transform.position = newPosition;
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

        if (IsJumping())
        {
            // Handle Jump input
            HandleJumpInput();
            IsVisiblyJumping = true;
        }
        else
        {
            IsVisiblyJumping = false;
        }

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
        float targetVelocity = inputDirection * topSpeed;
        float speedDiff = targetVelocity - rb.velocity.x;
        float accelerationRate = (Mathf.Abs(targetVelocity) > 0.01f)
            ? acceleration
            : deceleration;
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

    /// Adds an upward Impulse force to the RigidBody2D
    private void HandleJumpInput()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    /// Strengthens the RigidBody2D fall gravity by a factor of
    /// `fallGravityMultiplier`
    private void IncreaseFallGravity()
    {
        rb.gravityScale = legacyGravityScale * fallGravityMultiplier;
    }

    /// Resets the RigidBody2D's gravity scale to its original value
    private void ResetGravity()
    {
        this.rb.gravityScale = this.legacyGravityScale;
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
        this.sr.flipX = GetInputDirection() switch
        {
            -1 => true,
            1 => false,
            _ => this.sr.flipX
        };
    }

    private void HandleFoxHitsPlayer()
    {
        Debug.Log("I've been hit");
        if (this.isInvincible) return;

        isInvincible = true;
        EventManager.Events.PlayerInvincible();
        UIManager.ui.LoseHeart();
        Debug.Log("... but now I'm invincible so it does not matter");
        StartCoroutine(FlashEffect());
        
    }

    public IEnumerator FlashEffect()
    {
        for (int i = 0; i < flashCount; i++)
        {
            // Make the sprite transparent
            sr.color = new Color(1f, 1f, 1f, 0.5f);
            yield return new WaitForSeconds(this.flickDuration);

            // Make the sprite opaque
            // ReSharper disable once Unity.InefficientPropertyAccess
            sr.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(this.flickDuration);
        }

        this.isInvincible = false;
        EventManager.Events.PlayerVulnerable();
    }
}