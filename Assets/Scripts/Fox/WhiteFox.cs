using UnityEngine;

public class WhiteFox : Fox
{
  protected override void HandleMovement()
  {
    // Keep velocity
    this.rb.velocity = new Vector2(this.RunSpeed, this.rb.velocity.y);
  }

  protected override void Attack()
  {
    this.HasAttacked = true;
    this.HasInputJump = true;
  }
  
  protected override void SetAnimationParams()
  {
    SetRunAnimationParam(this.IsRunning());
    SetJumpAnimationParam(this.HasInputJump);
    SetFallAnimationParam(this.IsFalling());
  }

}