using JetBrains.Annotations;
using UnityEngine;

public class GrayFox : Fox
{
    private bool _hasInputDash;
    private bool _hasFinishedDash;
    private bool _isVisiblyDashing;

    [SerializeField] private float _dashForce = 3f;

    protected override void Update()
    {
        base.Update();

        // Carry on with normal Run if has started Attack
        if (_isVisiblyDashing) return;

        // Else, move when idle
        SetSpeedAsIdle();
    }

    protected override void HandleMovement()
    {
        // Assume normal run if the following is true
        if (!_hasFinishedDash) return;
        
        // Keep velocity
        rb.velocity = new Vector2(-this.RunSpeed, this.rb.velocity.y);
    }

    protected override void Attack()
    {
        HasAttacked = true;
        _hasInputDash = true;
    }

    /// Applies the dash force to the RigidBody2D, used as an Animation Event
    [UsedImplicitly]
    private void HandleDashStartAnimationEvent()
    {
        _isVisiblyDashing = true;

        // Used in Gray Fox animation event at start of Dash clip
        rb.AddForce(Vector2.left * _dashForce, ForceMode2D.Impulse);
    }

    /// Stops dash animation on call, used as an Animation Event
    [UsedImplicitly]
    private void HandleDashEndAnimationEvent()
    {
        _hasFinishedDash = true;
        _hasInputDash = false;
        _isVisiblyDashing = false;
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