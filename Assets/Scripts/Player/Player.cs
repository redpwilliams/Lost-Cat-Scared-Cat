using UnityEngine;

public class Player : Character
{
  internal static readonly float PlayerXPos = 0f;

  [Header("Movement Parameters")]
  [SerializeField] private float jumpForce;
  [SerializeField] private float topSpeed;
  [SerializeField] private float acceleration = 3.5f;
  [SerializeField] private float deceleration = 2f;

  [SerializeField] private BackgroundManager bgm;
  private new Transform transform;
  
  [Range(1, 5)]
  [SerializeField] private float fallGravityMultiplier;
  private float legacyGravityScale;

  private SpriteRenderer sr;

  protected override void Awake()
  {
    base.Awake();
    sr = GetComponent<SpriteRenderer>();
    transform = GetComponent<Transform>();
  }

  protected override void Start()
  {
    base.Start();
    legacyGravityScale = this.rb.gravityScale;

    // Put Player on specific location on screen
    Vector3 position = this.transform.position;
    gameObject.transform.position = new Vector3(PlayerXPos, position.y, position.z);
    
  }

  protected void Update()
  {
    
    // When Idle
    
    SetRunAnimationParam(IsVisiblyRunning);
    SetJumpAnimationParam(IsVisiblyJumping);
    SetFallAnimationParam(IsFalling());
    
    if (IsRunning() || !this.IsGrounded) return;
    float bgCurrentSpeed = this.bgm.GetScrollVelocity() * 1.1f;
    Vector3 currentPosition = transform.position;
    Vector3 newPosition =
      new Vector3(currentPosition.x - bgCurrentSpeed * Time.deltaTime,
        currentPosition.y, currentPosition.z);
    transform.position = newPosition;
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

  }

  protected override bool IsRunning()
  {
    return (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow));
  }

  private void HandleRunInput()
  {

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
