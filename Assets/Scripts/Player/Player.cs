using UnityEngine;

public class Player : Character
{

  protected override void FixedUpdate()
  {
    if (Input.GetKey(KeyCode.Space) && state == MovementState.Running)
      rb.velocity = Vector2.up * jumpVelocity;
  }

}
