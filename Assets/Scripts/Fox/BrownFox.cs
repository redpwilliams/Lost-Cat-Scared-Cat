using JetBrains.Annotations;
using UnityEngine;

public class BrownFox : Fox
{

    [SerializeField] private float _jumpForce = 3f;
    
    protected override void Update()
    {
        base.Update();
        
        // Carry on with normal Run if has started Attack
        if (IsVisiblyJumping) return;

        // Else, move when idle   
        SetSpeedAsIdle();
    }
    
    protected override void HandleMovement()
    {
        // Stay crouching if the following is true
        if (!this.IsVisiblyJumping) return;
        
        // Keep velocity
        this.rb.velocity = new Vector2(-this.RunSpeed, this.rb.velocity.y);
    }
    
    protected override void Attack()
    {
        this.HasAttacked = true;
        this.HasInputJump = true;
    }

    /// Applies the jump force to the RigidBody2D, used as an Animation Event
    [UsedImplicitly] protected override void HandleJumpAnimationEvent()
    {
      // Used in White Fox animation event at start of Jump clip
      rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse); 
      IsVisiblyJumping = true;
      HasInputJump = false;
    }
  
    protected override void SetAnimationParams()
    {
      SetRunAnimationParam(this.IsRunning());
      SetJumpAnimationParam(this.HasInputJump);
      SetFallAnimationParam(this.IsFalling());
      SetCrouchingAnimationParam(!this.HasAttacked);
    }
}