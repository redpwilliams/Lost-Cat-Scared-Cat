using UnityEngine;

public abstract class Character : MonoBehaviour
{
  [Header("Components")]
  [SerializeField] protected Rigidbody2D rb;
  [SerializeField] protected Animator anim;

  /// <summary>Upward velocity magnitude use for jump speed/height </summary>
  [SerializeField] protected float jumpVelocity;

  /// <summary>Boolean if the character is touching the main ground</summary>
  protected bool isGrounded;

  /// <summary>Name of the 'state' parameter in the Animator</summary>
  protected string stateParam;

  /// <summary>Current state of the animation</summary>
  protected enum MovementState
  {
    idle, running, jumping, falling,
  }

  /// <summary>Current animation state</summary>
  protected MovementState? state;

  /// <summary>Parameter name in the animator tab</summary>
  protected string animParameter;

  protected virtual void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    state = MovementState.running;
    isGrounded = true;
    stateParam = "state"; // "state" by default
  }

  protected virtual void Update()
  {
    // Set 'state' variable for current kinematics state
    if (isRunning())
      state = MovementState.running;
    else if (isJumping())
      state = MovementState.jumping;
    else if (isFalling())
      state = MovementState.falling;
    else
      state = null;
    UpdateAnimationState();
  }

  protected abstract void FixedUpdate();

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
  protected virtual bool isRunning()
  {
    return isGrounded; 
    /* NOTE
      This has to be temporary/overriden by a child class.
      This works in the main context of the game, but in places
      like a start menu or character select, the implementation
      might be different. 
    */
    /* REVIEW
      Consider a separate 'isGrounded' method, in addition to isRunning
    */ 
    
  }

  /// <summary>
  /// Returns the bool of the Character jumping 
  /// This is independent from the animation (animation relies on this/math)
  /// </summary>
  /// <returns>Status of Character jumping</returns>
  protected bool isJumping()
  {
    return rb.velocity.y > 0.01f;
  }

  /// <summary>
  ///  If GameObject is falling. Is independent from animation
  /// </summary>
  /// <returns>
  ///  rb.velocity.y, bool state if the Character is falling
  /// </returns>
  protected virtual bool isFalling()
  {
    Debug.Log("I'm still falling");
    return rb.velocity.y < -0.01f;
  }

  /// <summary>
  /// Called when this object collides with something
  /// Implementations must be handled by subclasses
  /// </summary>
  ///<param name="col">Collision2D object Player collided with</param>
  protected abstract void OnCollisionEnter2D(Collision2D col);

  /// <summary>
  /// Called when this object leaves a collider
  /// Implementations must be handled by subclasses
  /// </summary>
  ///<param name="col">Collision2D object Player exits collision with</param>
  protected abstract void OnCollisionExit2D(Collision2D col);
}
