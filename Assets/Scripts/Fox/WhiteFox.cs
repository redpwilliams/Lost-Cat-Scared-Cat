public class WhiteFox : Fox
{
  protected override void HandleMovement()
  {
    throw new System.NotImplementedException();
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