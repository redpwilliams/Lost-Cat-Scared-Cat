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

  protected override void Attack()
  {
    throw new System.NotImplementedException();
  }
}