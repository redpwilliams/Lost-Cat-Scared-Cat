using UnityEngine;

public class Player : Character
{

  void Start()
  {
    base.rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    state = MovementState.running;
    isGrounded = true;
    stateParam = "state";
  }

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
      state = null;
    base.UpdateAnimationState();
  }

  ///<param name="col">Collision2D object Player collided with</param>
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

  ///<param name="col">Collision2D object Player collided with</param>
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

}
