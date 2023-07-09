using UnityEngine;

public class RedFox : Fox
{
  private float jumpVelocity;

  protected override void Attack()
  {
    this.hasAttacked = true;
  }
}