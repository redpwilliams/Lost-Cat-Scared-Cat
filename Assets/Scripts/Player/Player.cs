using UnityEngine;

public class Player : Character
{
  /// Initial starting point of player
  internal const float PlayerXPos = 0f;

  [Header("Movement Parameters")]
  [SerializeField] private float jumpForce; 
  [SerializeField] private float topSpeed;
  [SerializeField] private float acceleration = 4f; 
  [SerializeField] private float deceleration = 10f; // Bigger value = harder stop

  /// How much to change the gravity when Player is falling
  [Range(1, 5)]
  [SerializeField] private float fallGravityMultiplier;
  
  /// "Normal" gravity when not jumping
  private float legacyGravityScale;
  
  /// BackgroundManager for access to scroll velocity
  [SerializeField] private BackgroundManager bgm;
  
  private new Transform transform;
  private SpriteRenderer sr;

  protected override void Awake()
  {
    base.Awake();
    sr = GetComponent<SpriteRenderer>();
    transform = GetComponent<Transform>();
    legacyGravityScale = this.rb.gravityScale;
  }

  protected void Start()
  {
    // Put Player on specific location on screen
    Vector3 position = this.transform.position;
    this.transform.position = new Vector3(PlayerXPos, position.y, position.z);
  }

  protected void Update()
  {
    // Set animation parameters
    SetRunAnimationParam(IsVisiblyRunning);
    SetJumpAnimationParam(IsVisiblyJumping);
    SetFallAnimationParam(IsFalling());
    
    // Return if Player is in any motion
    if (IsRunning() || !this.IsGrounded || this.rb.velocity.x > 0.1f) return;
    
    // TODO - Reference specific background instead of direct parallax coefficient
    float bgCurrentSpeed = this.bgm.GetScrollVelocity() * 1.1f;
    
    // Set new position using Transform component (avoids material friction)
    Vector3 currentPosition = transform.position;
    Vector3 newPosition =
      new Vector3(currentPosition.x - bgCurrentSpeed * Time.deltaTime,
        currentPosition.y, currentPosition.z);
    transform.position = newPosition;
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
    } else { IsVisiblyJumping = false; }

    // Conditionally Handle Fall
    if (IsFalling()) HandleFall();
    else ResetGravity();
  }
  
  protected override bool IsRunning()
  {
    return (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow));
  }

  private void HandleRunInput(int inputDirection)
  {
    // Force-based movement
    float targetVelocity = inputDirection * topSpeed;
    float speedDiff = targetVelocity - rb.velocity.x;
    float accelerationRate = (Mathf.Abs(targetVelocity) > 0.01f) ? acceleration : deceleration;
    float movement = Mathf.Abs(speedDiff) * accelerationRate * Mathf.Sign(speedDiff);
    rb.AddForce(movement * Vector2.right);

    // Switch direction
    HandleFlipSprite();
  }

  protected override bool IsJumping()
  {
    return (Input.GetKey(KeyCode.Space) && this.IsGrounded);
  }

  private void HandleJumpInput()
  {
    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
  }

  private void HandleFall()
  {
      rb.gravityScale = legacyGravityScale * fallGravityMultiplier;
  }

  private void ResetGravity()
  {
    this.rb.gravityScale = this.legacyGravityScale;
  }

  private static int GetInputDirection()
  {
    if (Input.GetKey(KeyCode.RightArrow)) return 1;
    if (Input.GetKey(KeyCode.LeftArrow)) return -1;
    return 0;
  }

  private void HandleFlipSprite()
  {
    this.sr.flipX = GetInputDirection() switch
    {
      -1 => true,
      1 => false,
      _ => this.sr.flipX
    };
  }
}
