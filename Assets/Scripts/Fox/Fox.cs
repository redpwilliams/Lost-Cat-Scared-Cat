using UnityEngine;

/// <summary>
/// Represents all foxes. 
/// All sub-instances either jump or don't jump

///</summary>
public abstract class Fox : Character
{

  // Then, variable horizontal velocity for normal running foxes
  [SerializeField] protected float runSpeed = -3f;
  protected float sitSpeed = -1.5f;

  /// <summary> Fox will self destruct after reaching this value </summary>
  private readonly float deadZone = -4.5f;

  /// <summary> Fox only attacks once </summary>
  protected bool hasAttacked = false;

  /// <summary> The amount of space between the Fox and Player to initiate Fox attack</summary>
  // TODO - I could change this into an array to accommodate for sitting foxes
  // Each value in the array would dictate the spacing before each action of a sitting fox
  protected float spaceBeforeAttack = 1.5f;

  protected bool isVisiblyJumping = false;

  private static readonly int Running = Animator.StringToHash("IsRunning");
  private static readonly int Jumping = Animator.StringToHash("IsJumping");
  private static readonly int Falling = Animator.StringToHash("IsFalling");

  protected override void Start()
  {
    base.Start();
    Rigidbody2D rb2d = base.rb;
    // Flip sprite horizontally
    Transform rbTransform = rb2d.transform;
    Vector3 localScale = rbTransform.localScale;
    localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
    rbTransform.localScale = localScale;
  }

  protected override void Update()
  {
    // Destroy on off-screen
    if (IsOffscreen(transform.position.x, deadZone)) 
    {
      Destroy(gameObject);
      return;
    }

    // Update animation
    this.anim.SetBool(Running, this.IsRunning());
    this.anim.SetBool(Jumping, isVisiblyJumping); // I delayed the jump impulse force to line up with the jump animation
    this.anim.SetBool(Falling, this.IsFalling());
  }

  protected override void FixedUpdate()
  {
    // Distance between Fox and Player's position && isRunning
    float distanceFromPlayer = Mathf.Abs(Player.PLAYER_X_POS - transform.position.x);
    if (hasAttacked || (!IsInPosition(distanceFromPlayer, this.spaceBeforeAttack)) ||
        this.state != MovementState.Running) return;
    
    this.isVisiblyJumping = true;
    Attack();
  }

  /// <summary>
  /// A Fox's attack sequence
  /// </summary>
  protected abstract void Attack();

  bool IsOffscreen(float currPos, float deadZone)
  {
    return currPos < deadZone;
  }

  bool IsInPosition(float distance, float spacing)
  {
    return distance < spacing;
  }

}