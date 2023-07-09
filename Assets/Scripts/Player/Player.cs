using UnityEngine;

public class Player : Character
{
  internal static readonly float PlayerXPos = 0f;

  [Header("Movement Parameters")]
  [SerializeField] private float jumpForce;
  [SerializeField] private float topSpeed;
  
  [Range(1, 5)]
  [SerializeField] private float fallGravityMultiplier;
  private float legacyGravityScale;

  [Header("Aligned Background Game Object")]
  [Tooltip("Moves the Player at the same negative velocity of the provided background while Player is Idle")]
  [SerializeField] private GameObject alignedBackground;

  private SpriteRenderer sr;
  private Rigidbody2D alignedBgRigidBody;

  protected override void Awake()
  {
    base.Awake();
    sr = GetComponent<SpriteRenderer>();
  }

  protected override void Start()
  {
    this.alignedBgRigidBody = this.alignedBackground.GetComponent<Rigidbody2D>();
    base.Start();
    legacyGravityScale = this.rb.gravityScale;

    // Put Player on specific location on screen
    Vector3 position = this.transform.position;
    gameObject.transform.position = new Vector3(PlayerXPos, position.y, position.z);
  }

  protected void Update()
  {
    SetRunAnimationParam(IsVisiblyRunning);
    SetJumpAnimationParam(IsVisiblyJumping);
    SetFallAnimationParam(IsFalling());
  }

  protected void FixedUpdate()
  {
    // Handle Run input
    if (IsRunning())
    {
      IsVisiblyRunning = true;
      HandleRunInput();
    } else { IsVisiblyRunning = false; }

    if (IsJumping())
    {
      // Handle Jump input
      HandleJumpInput();
      IsVisiblyJumping = true;
    } else { IsVisiblyJumping = false; }

    // Conditionally Handle Fall
    if (IsFalling()) HandleFall();
    else ResetGravity();

    // When Idle
    if (!IsRunning() && this.IsGrounded) 
    { 
      float bgCurrentSpeed = this.alignedBgRigidBody.velocity.x;
      rb.velocity = new Vector2(bgCurrentSpeed, rb.velocity.y);
    }

    // rb.drag = (IsJumping() || IsFalling()) ? aerialDrag : 0;
  }

  protected override bool IsRunning()
  {
    return (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow));
  }

  private void HandleRunInput()
  {
    float acceleration = 3.5f;
    float deceleration = 2f;

    // Force-based movement
    float targetVelocity = GetInputDirection() * topSpeed;
    float speedDiff = targetVelocity - rb.velocity.x;
    float accelerationRate = (Mathf.Abs(targetVelocity) > 0.01f) ? acceleration : deceleration;
    float movement = Mathf.Abs(speedDiff) * accelerationRate * Mathf.Sign(speedDiff);
    rb.AddForce(movement * Vector2.right);

    // Switch direction
    HandleFlipSprite();
   
    // Player is now running/in motion
    IsVisiblyRunning = true;
  }

  protected override bool IsJumping()
  {
    return (Input.GetKey(KeyCode.Space) && this.IsGrounded);
  }

  private void HandleJumpInput()
  {
    if (IsJumping())
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

  private float GetInputDirection()
  {
    if (Input.GetKey(KeyCode.RightArrow)) return 1;
    else if (Input.GetKey(KeyCode.LeftArrow)) return -1;
    return 0;
  }

  private void HandleFlipSprite()
  {
    switch(GetInputDirection())
    {
      case -1: 
        sr.flipX = true;
        break;

      case 1:
        sr.flipX = false;
        break;
    }
  }


}
