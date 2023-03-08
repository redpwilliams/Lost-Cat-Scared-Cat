using UnityEngine;


/// <summary>
/// Foxes that run in the foreground. Can either jump or lunge 
/// </summary>
public class ForegroundFox : Fox
{
 
  protected override void Start()
  {
    base.Start();
    Debug.LogFormat("This foreground fox {0} a jumping fox", isJumpingFox ? "is" : "is not");
  }

  protected override void Update()
  {
    base.Update();
  }

  protected override void FixedUpdate()
  {
    if (Input.GetKey(KeyCode.F) && state == MovementState.running)
    {
    Debug.Log(state);
      rb.velocity = Vector2.up * jumpVelocity;
    }
  }

  // void Update()
  // {
  //   if (isJumping())
  //     state = MovementState.jumping;
  //   else if (isFalling())
  //     state = MovementState.falling;
  //   else if (isRunning())
  //     state = MovementState.running;
  //   else
  //     state = null;
  //   base.UpdateAnimationState();
  // }

  protected override void Attack()
  {
    throw new System.NotImplementedException();
  }
}