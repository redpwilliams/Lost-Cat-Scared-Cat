using UnityEngine;

public class WhiteFox : Fox
{

  private float jumpVelocity;

  protected override void Awake()
  {
    base.Awake();
    jumpVelocity = 3f;
  }

  protected override void Start()
  {
    base.Start();
  }

  protected override void Update()
  {
    base.Update();
    rb.velocity = new Vector2(runSpeed, rb.velocity.y);
  }

  protected override void FixedUpdate()
  {
    base.FixedUpdate();
  }

  protected override void Attack()
  {
    hasAttacked = true;
    rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
  }
}