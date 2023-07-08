using UnityEngine;

public class WhiteFox : Fox
{

  private AnimationState animState;

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
  }

  protected override void FixedUpdate()
  {
    rb.velocity = new Vector2(runSpeed, rb.velocity.y);
    base.FixedUpdate();
    Debug.Log(state);
  }

  protected override void Attack()
  {
    hasAttacked = true;
    Debug.Log("Jumped");
  }

  [SerializeField]
  private void HandleJump()
  {
    rb.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
    isVisiblyJumping = false;
  }
}