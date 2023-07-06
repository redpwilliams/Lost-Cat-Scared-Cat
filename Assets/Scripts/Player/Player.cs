using UnityEngine;

public class Player : Character
{
  public static float PLAYER_X_POS = 0f;

  [Header("Movement Parameters")]
  [SerializeField] private float jumpForce;
  [SerializeField] private float topSpeed;
  [SerializeField] private float aerialDrag;
  
  [Range(1, 5)]
  [SerializeField] private float fallGravityMultiplier;
  private float legacyGravityScale;

  [Header("Aligned Background Game Object")]
  [Tooltip("Moves the Player at the same negative velocity of the provided background")]
  [SerializeField] private GameObject alignedBackground;

  private SpriteRenderer sr;

  protected override void Awake()
  {
    base.Awake();
    sr = GetComponent<SpriteRenderer>();
  }

  protected override void Start()
  {
    base.Start();
    state = MovementState.Idle;
    legacyGravityScale = this.rb.gravityScale;

    // Put Player on specific location on screen
    gameObject.transform.position = new Vector3(PLAYER_X_POS, this.transform.position.y, this.transform.position.z);
  }

  protected override void FixedUpdate()
  {
    // Handle Run input
    HandleRun();

    // Handle Jump input
    HandleJump();

    // Handle Fall input
    HandleFall();

    if (!IsRunning() && this.isGrounded) 
    { 
      float bgCurrentSpeed = alignedBackground.GetComponent<Rigidbody2D>().velocity.x;
      rb.velocity = new Vector2(bgCurrentSpeed, rb.velocity.y);
    }

    rb.drag = (IsJumping() || IsFalling()) ? aerialDrag : 0;

    Debug.Log(this.state);
  }

  protected override bool IsRunning()
  {
    return (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow));
  }

  private void HandleRun()
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

  }

  private void HandleJump()
  {
    if (Input.GetKey(KeyCode.Space) && this.isGrounded)
      rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
  }

  private void HandleFall()
  {
    if (this.IsFalling())
      rb.gravityScale = legacyGravityScale * fallGravityMultiplier;
    else
      rb.gravityScale = legacyGravityScale;

    // if (this.IsJumping() && !Input.GetKey(KeyCode.Space))
    //   rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.25f);
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
