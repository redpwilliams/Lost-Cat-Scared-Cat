using UnityEngine;

public class WhiteFox : Fox
{

  private AnimationState animState;

  protected override void Attack()
  {
    this.hasAttacked = true;
    this.HasInputJump = true;
  }

}