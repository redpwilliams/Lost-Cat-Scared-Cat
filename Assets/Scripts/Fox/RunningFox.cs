using UnityEngine;

public class RunningFox : Fox
{

  protected override void Awake()
  {
    base.Awake();
  }

  protected override void Start()
  {
    base.Start();
  }

  protected override void FixedUpdate()
  {
    base.FixedUpdate();
  }

  protected override void Attack()
  {
    hasAttacked = true;
    rb.velocity = new Vector2(rb.velocity.x, jumpVelocity); // Just jumps
  }
}