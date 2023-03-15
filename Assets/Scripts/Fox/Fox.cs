using UnityEngine;

/// <summary>
/// Represents all foxes. 
/// All sub-instances either jump or don't jump

///</summary>
public abstract class Fox : Character
{
  /// <summary>if false, it's a lunging fox</summary>
  protected bool isJumpingFox;

  /// <summary> The Fox's horizontal velocity </summary>
  [SerializeField] protected float horizontalVelocity = 2f;

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

  protected override void FixedUpdate()
  {
    if (Input.GetKey(KeyCode.F) && state == MovementState.Running)
    {
      // Keep horizontal velocity, change vertical velocity
      rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
    }
    // Use the "previous state's" velocity vector
    rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
  }

  /// <summary>
  /// A Fox's attack sequence
  /// </summary>
  protected abstract void Attack();

}