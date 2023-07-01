using UnityEngine;

public class Player : Character
{
  public static float PLAYER_X_POS = 0f;

  [SerializeField] private float jumpForce;
  [SerializeField] private float topSpeed;
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

    // Put Player on specific location on screen
    gameObject.transform.position = new Vector3(PLAYER_X_POS, this.transform.position.y, this.transform.position.z);
  }

  protected override void FixedUpdate()
  {
    // Handle Run input
    HandleRun();

    // Handle Jump input
    HandleJump();

    if (!IsRunning()) 
    { 
      float bgCurrentSpeed = alignedBackground.GetComponent<Rigidbody2D>().velocity.x;
      rb.velocity = new Vector2(bgCurrentSpeed, rb.velocity.y);
    }
  }

  protected override bool IsRunning()
  {
    return (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow));
  }

  private void HandleRun()
  {
    float acceleration = 2.5f;
    float deceleration = 2f;

    // Force-based movement
    float targetSpeed = GetInputDirection() * topSpeed;
    float speedDiff = targetSpeed - rb.velocity.x;
    float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
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
