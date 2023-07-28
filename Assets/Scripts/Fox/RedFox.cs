using Unity.VisualScripting;
using UnityEngine;

public class RedFox : Fox
{
  protected override void HandleMovement()
  {
    // Keep velocity
    this.rb.velocity = new Vector2(-this.RunSpeed, this.rb.velocity.y);
  }
  
  protected override void Attack()
  {
    this.HasAttacked = true;
  }

  protected override void SetAnimationParams()
  {
    SetRunAnimationParam(this.IsRunning());
  }

  protected override void HandleJumpAnimationEvent(float jumpForce)
  {
    throw new System.NotImplementedException();
  }
}