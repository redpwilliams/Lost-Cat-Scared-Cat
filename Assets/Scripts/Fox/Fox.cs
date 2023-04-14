using UnityEngine;

/// <summary>
/// Represents all foxes. 
/// All sub-instances either jump or don't jump

///</summary>
public class Fox : Character
{

  /// <summary> The Fox's horizontal velocity </summary>
  protected float horizontalVelocity = -3f;
  // NOTE -1.5f for "sitting" so it moves along with the background
  // Then, variable horizontal velocity for normal running foxes

  /// <summary> Fox will self destruct after reaching this value </summary>
  public static float deadZone = -4.5f;

  /// <summary> Fox only attacks once </summary>
  protected bool hasAttacked = false;

  protected override void Start()
  {
    base.Start();
    // Determine if the Fox will jump
    Rigidbody2D rb2d = base.rb;
    rb2d.transform.localScale = new Vector3(-rb2d.transform.localScale.x, rb2d.transform.localScale.y, rb2d.transform.localScale.z);
  }

  protected override void Update()
  {
    base.Update(); // ForegroundFox and BackgroundFox need to call Update() of Character
    // Destroy on off-screen
    if (transform.position.x < deadZone) Destroy(gameObject);
  }

  protected override void FixedUpdate()
  {
    // Distance between Fox and Player's position && isRunning
    if (!hasAttacked && Mathf.Abs(Player.PLAYER_X_POS - transform.position.x) < 1.5f && state == MovementState.Running)
    {
      // Keep horizontal velocity, change vertical velocity
      // rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
      Attack();
    }
    // Use the "previous state's" velocity vector
    rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
  }

  /// <summary>
  /// A Fox's attack sequence
  /// </summary>
  protected void Attack()
  {
    hasAttacked = true;
    rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
  }

}