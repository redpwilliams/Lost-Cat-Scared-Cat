using UnityEngine;

public class PlayerScript : Character
{

  private string stateParam;

  void Start()
  {
    base.rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    state = MovementState.running;
    isGrounded = true;
    stateParam = "state";
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    if (Input.GetKey(KeyCode.Space) && state == MovementState.running)
      rb.velocity = Vector2.up * jumpVelocity;
  }

  void Update()
  {
    if (isJumping())
      state = MovementState.jumping;
    else if (isFalling())
      state = MovementState.falling;
    else if (isRunning())
      state = MovementState.running;
    else
      state = MovementState.nil; // Fixes jump between jump and fall animation
    UpdateAnimationState();
  }

  void OnCollisionEnter2D(Collision2D col)
  {
    isGrounded = true;
    switch (LayerMask.LayerToName(col.gameObject.layer))
    {
      case "Ground":
        // Do something
        break;

      default:
        // Do something
        break;
    }
  }

  void OnCollisionExit2D(Collision2D col)
  {
    isGrounded = false;
    switch (LayerMask.LayerToName(col.gameObject.layer))
    {
      case "Ground":
        // Do something
        break;

      default:
        // Do something
        break;
    }
  }

  // Updates the animation state in the Animator
  protected override void UpdateAnimationState()
  {
    anim.SetInteger(stateParam, (int)state);
  }
}
