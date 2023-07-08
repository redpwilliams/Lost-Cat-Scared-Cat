using UnityEngine;

public class WhiteFox : Fox
{

  private AnimationState animState;

  protected override void FixedUpdate()
  {
    rb.velocity = new Vector2(runSpeed, rb.velocity.y);
    base.FixedUpdate();
  }

  protected override void Attack()
  {
    hasAttacked = true;
  }

  [SerializeField]
  private void HandleJump()
  {
    rb.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
    isVisiblyJumping = false;
  }
}