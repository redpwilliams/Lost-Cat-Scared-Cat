using UnityEngine;

public class WhiteFox : Fox
{

  protected override void Awake()
  {
    base.Awake();
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
  }
}