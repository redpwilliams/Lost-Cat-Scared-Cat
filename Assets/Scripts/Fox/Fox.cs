using UnityEngine;

/// <summary>
/// Represents all foxes. 
/// All sub-instances either jump or don't jump

///</summary>
public abstract class Fox : Character
{
  /// <summary>Fox default run speed</summary>
  [SerializeField] private float runSpeed = -3f;
  protected float sitSpeed = -1.5f;

  /// <summary> Fox will self destruct after reaching this value </summary>
  private readonly float deadZone = -4.5f;

  /// <summary> Fox only attacks once </summary>
  protected bool hasAttacked = false;

  /// <summary> The amount of space between the Fox and Player to initiate Fox attack</summary>
  // TODO - I could change this into an array to accommodate for sitting foxes
  // Each value in the array would dictate the spacing before each action of a sitting fox
  private readonly float spaceBeforeAttack = 1.5f;

  protected override void Start()
  {
    base.Start();
    // Flip sprite horizontally
    Transform rbTransform = this.rb.transform;
    Vector3 localScale = rbTransform.localScale;
    localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
    rbTransform.localScale = localScale;
  }

  protected void Update()
  {
    // Destroy on off-screen
    if (IsOffscreen(transform.position.x, deadZone)) 
    {
      Destroy(gameObject);
      return;
    }

    // Update animation
    SetRunAnimationParam(this.IsRunning());
    SetJumpAnimationParam(this.HasInputJump);
    SetFallAnimationParam(this.IsFalling());
  }

  protected void FixedUpdate()
  {
    // Keep velocity
    this.rb.velocity = new Vector2(this.runSpeed, this.rb.velocity.y);
    
    // Distance between Fox and Player's position && isRunning
    float distanceFromPlayer = Mathf.Abs(Player.PlayerXPos - transform.position.x);
    if (hasAttacked || (!IsInPosition(distanceFromPlayer, this.spaceBeforeAttack)) 
        ) return;
    
    Attack();
  }

  /// <summary>
  /// A Fox's attack sequence
  /// </summary>
  protected abstract void Attack();

  private static bool IsOffscreen(float currPos, float deadZone)
  {
    return currPos < deadZone;
  }

  bool IsInPosition(float distance, float spacing)
  {
    return distance < spacing;
  }

}