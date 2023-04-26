using UnityEngine;

/// <summary>
/// Represents all foxes. 
/// All sub-instances either jump or don't jump

///</summary>
public abstract class Fox : Character
{

  /// <summary> The Fox's horizontal velocity </summary>
  protected float horizontalVelocity = -3f;
  // NOTE -1.5f for "sitting" so it moves along with the background
  // Then, variable horizontal velocity for normal running foxes

  /// <summary> Fox will self destruct after reaching this value </summary>
  public static float deadZone = -4.5f;

  /// <summary> Fox only attacks once </summary>
  protected bool hasAttacked = false;

  /// <summary> The amount of space between the Fox and Player to initiate Fox attack
  // TODO - I could change this into an array to accommodate for sitting foxes
  // Each value in the array would dictate the spacing before each action of a sitting fox
  protected float spaceBeforeAttack = 1.5f;

  protected override void Awake()
  {
    base.Awake();
  }

  protected override void Start()
  {
    base.Start();
    Rigidbody2D rb2d = base.rb;
    // Flip sprite horizontally
    rb2d.transform.localScale = new Vector3(-rb2d.transform.localScale.x, rb2d.transform.localScale.y, rb2d.transform.localScale.z);
  }

  protected override void Update()
  {
    base.Update();

    // Destroy on off-screen
    if (isOffscreen(transform.position.x, deadZone)) Destroy(gameObject);
  }

  protected override void FixedUpdate()
  {
    // Distance between Fox and Player's position && isRunning
    float distanceFromPlayer = Mathf.Abs(Player.PLAYER_X_POS - transform.position.x); 
    if (!hasAttacked && (distanceFromPlayer < spaceBeforeAttack) && state == MovementState.Running)
    {
      Attack();
    }
    // Use the "previous state's" velocity vector
    rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
  }

  /// <summary>
  /// A Fox's attack sequence
  /// </summary>
  protected abstract void Attack();

  bool isOffscreen(float currPos, float deadZone)
  {
    return currPos < deadZone;
  }

}