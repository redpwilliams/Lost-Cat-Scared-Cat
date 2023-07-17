using JetBrains.Annotations;
using UnityEngine;

public class GrayFox : Fox
{

  // private bool isVisiblyDashing;
  private bool hasInputDash;
  
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
  
  /// Applies the dash force to the RigidBody2D, used as an Animation Event
  [UsedImplicitly] private void HandleDashAnimationEvent(float dashForce)
  {
    // Used in White Fox animation event at start of Jump clip
    rb.AddForce(Vector2.right * dashForce, ForceMode2D.Impulse); 
    // this.isVisiblyDashing = true;
    this.hasInputDash = false;
  }
  
  protected override void SetAnimationParams()
  {
    SetRunAnimationParam(this.IsRunning());
    SetJumpAnimationParam(this.hasInputDash);
  }

}