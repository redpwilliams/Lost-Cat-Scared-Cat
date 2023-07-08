using UnityEngine;

public abstract class Character : MonoBehaviour
{
  [Header("Components")]
  [SerializeField] protected Rigidbody2D rb;
  [SerializeField] protected Animator anim;

  /// <summary>Boolean if the character is touching the main ground</summary>
  protected bool isGrounded;

  /// <summary>Name of the 'state' parameter in the Animator</summary>
  protected string stateParam;

  /// <summary>Current state of the animation</summary>
  protected enum MovementState
  {
    Idle, Running, Jumping, Falling, Landing
  }

  /// <summary>Current animation state</summary>
  protected MovementState? state;

  /// <summary>Parameter name in the animator tab</summary>
  protected string animParameter;

  protected virtual void Awake()
  {
    isGrounded = true;
    stateParam = "state"; // "state" by default
  }

  protected virtual void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    state = MovementState.Running;
  }

  protected virtual void Update()
  {
  }

  protected virtual void FixedUpdate()
  {
    // Set 'state' variable for current kinematics state
    if (IsJumping())
      state = MovementState.Jumping;
    else if (IsFalling())
      state = MovementState.Falling;
    // else if (state == MovementState.Falling)
    //   state = MovementState.Landing;
    else if (IsRunning() && this.isGrounded)
    {
      state = MovementState.Running;
      Debug.Log("Is grounded, chose is grounded");
    }
    else if (!isGrounded)
      state = MovementState.Idle;
    else
      state = null;
    UpdateAnimationState();

  }

  /// <summary>
  /// Updates the animation state through the animator tab
  /// </summary>
  protected virtual void UpdateAnimationState()
  {
    if (state == null) return; // No update when in between transitions
    anim.SetInteger(stateParam, (int)state);
  }

  /// <summary>
  /// Returns whether or not the character is running
  /// </summary>
  /// <returns>isGrounded, bool value of character grounded status</returns>
  protected virtual bool IsRunning()
  {
    return this.isGrounded && Mathf.Abs(this.rb.velocity.x) > 0.1f;
  }

  /// <summary>
  /// Returns the bool of the Character jumping 
  /// This is independent from the animation (animation relies on this/math)
  /// </summary>
  /// <returns>Status of Character jumping</returns>
  protected virtual bool IsJumping()
  {
    return rb.velocity.y > 0.01f;
  }

  /// <summary>
  ///  If GameObject is falling. Is independent from animation
  /// </summary>
  /// <returns>
  ///  rb.velocity.y, bool state if the Character is falling
  /// </returns>
  protected virtual bool IsFalling()
  {
    return rb.velocity.y < -0.1f;
  }

  /// <summary>
  /// Called when this object collides with something
  /// </summary>
  ///<param name="col">Collision2D object Character collided with</param>
  private void OnCollisionEnter2D(Collision2D col)
  {
    isGrounded = true;
  }

  /// <summary>
  /// Called when this object leaves a collider
  /// </summary>
  ///<param name="col">Collision2D object Character exits collision with</param>
  private void OnCollisionExit2D(Collision2D col)
  {
    isGrounded = false;
  }
}
