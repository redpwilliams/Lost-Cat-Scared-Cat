using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
  protected Rigidbody2D rb;
  private Animator anim;

  protected bool IsGrounded { get; private set; }

  protected bool IsVisiblyJumping { get; set; }

  private bool isVisiblyRunning;

  protected bool IsVisiblyRunning { get; set; }

  protected bool HasInputJump { get; set; }

  private static readonly int Running = Animator.StringToHash("IsRunning");
  private static readonly int Jumping = Animator.StringToHash("IsJumping");
  private static readonly int Falling = Animator.StringToHash("IsFalling");
  
  protected virtual void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    
    this.IsGrounded = true;
    this.HasInputJump = false;
    this.IsVisiblyJumping = false;
  }
  
  // protected virtual void Start()
  // {
  // }

  /// <summary>
  /// Returns whether or not the character is running
  /// </summary>
  /// <returns>isGrounded, bool value of character grounded status</returns>
  protected virtual bool IsRunning()
  {
    return this.IsGrounded && Mathf.Abs(this.rb.velocity.x) > 0.1f;
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
  protected bool IsFalling()
  {
    return rb.velocity.y < -0.1f;
  }

  protected void SetRunAnimationParam(bool value)
  {
    this.anim.SetBool(Running, value);
  }

  protected void SetJumpAnimationParam(bool value)
  {
    this.anim.SetBool(Jumping, value);
  }

  protected void SetFallAnimationParam(bool value)
  {
    this.anim.SetBool(Falling, value);
  }
  
  /// <summary>
  /// Called when this object collides with something
  /// </summary>
  private void OnCollisionEnter2D()
  {
    this.IsGrounded = true;
  }

  /// <summary>
  /// Called when this object leaves a collider
  /// </summary>
  private void OnCollisionExit2D()
  {
    this.IsGrounded = false;
  }
}
