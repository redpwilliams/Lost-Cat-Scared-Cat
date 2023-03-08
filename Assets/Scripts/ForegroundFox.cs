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

  protected override void Attack()
  {
    throw new System.NotImplementedException();
  }

  //NOTE - ForegroundFox and BackgroundFox will implement Attack() differently
}