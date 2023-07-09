using JetBrains.Annotations;
using UnityEngine;

public class WhiteFox : Fox
{

  private AnimationState animState;

  protected override void Attack()
  {
    this.hasAttacked = true;
    this.HasInputJump = true;
  }

  [UsedImplicitly] private void HandleJump()
  {
    // Used in White Fox animation event at start of Jump clip
    rb.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
    isVisiblyJumping = true;
    HasInputJump = false;
  }

}