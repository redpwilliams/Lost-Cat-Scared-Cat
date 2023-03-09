using UnityEngine;

/// <summary>
/// Represents all foxes. 
/// All sub-instances either jump or don't jump

///</summary>
public abstract class Fox : Character
{
  /// <summary>if false, it's a lunging fox</summary>
  protected bool isJumpingFox;

  
  protected override void Start()
  {
    base.Start();
    // Determine if the Fox will jump
    isJumpingFox = Random.Range(0, 2) == 1;
  }

  protected override void Update()
  {
    base.Update(); // ForegroundFox and BackgroundFox need to call Update() of Character
  }

  /// <summary>
  /// A Fox's attack sequence
  /// </summary>
  protected abstract void Attack();

  protected override void OnCollisionEnter2D(Collision2D col)
  {
    isGrounded = true;
    switch (LayerMask.LayerToName(col.gameObject.layer))
    {
      case "Ground":
        // Do something
        break;

      default:
        // Do something
        break;
    }
  }

  protected override void OnCollisionExit2D(Collision2D col)
  {
    isGrounded = false;
    switch (LayerMask.LayerToName(col.gameObject.layer))
    {
      case "Ground":
        // Do something
        break;

      default:
        // Do something
        break;
    }
  }
}