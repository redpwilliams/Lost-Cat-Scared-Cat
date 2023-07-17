using JetBrains.Annotations;
using UnityEngine;

public class GrayFox : Fox
{

  private bool hasInputDash;
  
  protected override void HandleMovement()
  {
    if (this.hasInputDash) return;
    // Keep velocity
    this.rb.velocity = new Vector2(this.RunSpeed, this.rb.velocity.y);
  }

  protected override void Attack()
  {
    this.HasAttacked = true;
    this.hasInputDash = true;
  }
  
  /// Applies the dash force to the RigidBody2D, used as an Animation Event
  [UsedImplicitly] private void HandleDashStartAnimationEvent(float dashForce)
  {
    // Used in White Fox animation event at start of Jump clip
    // FIXME - HandleMovement() is what kills this, consider adding parameter that turns on/off line 12
    rb.AddForce(Vector2.left * dashForce, ForceMode2D.Impulse); 
  }

  /// Stops dash animation on call, used as an Animation Event
  [UsedImplicitly] private void HandleDashEndAnimationEvent()
  {
    this.hasInputDash = false;
  }
  
  protected override void SetAnimationParams()
  {
    SetRunAnimationParam(this.IsRunning());
    SetDashingAnimationParam(this.hasInputDash);
  }

}