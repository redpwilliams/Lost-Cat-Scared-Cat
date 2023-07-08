using UnityEngine;

public class RedFox : Fox
{
  private float jumpVelocity;

  protected override void Update()
  {
    base.Update();
    this.rb.velocity = new Vector2(this.runSpeed, this.rb.velocity.y);
  }

  protected override void Attack()
  {
    this.hasAttacked = true;
  }
}