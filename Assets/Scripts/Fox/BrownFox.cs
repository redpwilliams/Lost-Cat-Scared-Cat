using UnityEngine;

public class BrownFox : Fox
{
    private new Transform transform;

    protected override void Awake()
    {
       base.Awake();
       this.transform = GetComponent<Transform>();
    }
    
    protected override void Update()
    {
        base.Update();
        
        // Carry on with normal Run if has started Attack
        if (IsVisiblyJumping) return;

        // Else, move when idle   
        float idleSpeed = BackgroundManager.bgm.GetScrollVelocity() * 1.1f; // FIXME - Get 1.1f dynamically
        // ^ Singleton?
        Vector3 currentPosition = transform.position;
        this.transform.position = new Vector3(currentPosition.x - idleSpeed * Time.deltaTime,
                currentPosition.y, currentPosition.z);
    }
    
    protected override void HandleMovement()
    {
        // Stay crouching if the following is true
        if (!this.IsVisiblyJumping) return;
        
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
      SetCrouchingAnimationParam(!this.HasAttacked);
    }
}