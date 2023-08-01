using JetBrains.Annotations;
using UnityEngine;

public class GrayFox : Fox
{
    private bool _hasInputDash;
    private bool _isVisiblyDashing;

    protected override void Update()
    {
        base.Update();

        // Carry on with normal Run if has started Attack
        if (this._isVisiblyDashing) return;

        // Else, move when idle
        SetSpeedAsIdle();
    }

    protected override void HandleMovement()
    {
        // Stay crouching if the following is true
        if (!this._isVisiblyDashing) return;

        // Keep velocity
        this.rb.velocity = new Vector2(-this.RunSpeed, this.rb.velocity.y);
    }

    protected override void Attack()
    {
        this.HasAttacked = true;
        this._hasInputDash = true;
    }

    /// Applies the dash force to the RigidBody2D, used as an Animation Event
    [UsedImplicitly]
    private void HandleDashStartAnimationEvent(float dashForce)
    {
        this._isVisiblyDashing = true;

        // Used in White Fox animation event at start of Jump clip
        rb.AddForce(Vector2.left * dashForce, ForceMode2D.Impulse);
    }

    /// Stops dash animation on call, used as an Animation Event
    [UsedImplicitly]
    private void HandleDashEndAnimationEvent()
    {
        this._hasInputDash = false;
    }

    protected override void HandleJumpAnimationEvent()
    {
        throw new System.NotImplementedException();
    }

    protected override void SetAnimationParams()
    {
        SetRunAnimationParam(this.IsRunning());
        SetCrouchingAnimationParam(!this.HasAttacked);
        SetDashingAnimationParam(this._hasInputDash);
    }
}