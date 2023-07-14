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
  /// Returns whether or not the character is running
  protected virtual bool IsRunning()
  {
    return this.IsGrounded && Mathf.Abs(this.rb.velocity.x) > 0.1f;
  }

  /// Returns the bool of the Character jumping 
  /// This is independent from the animation (animation relies on this/math)
  protected virtual bool IsJumping()
  {
    return rb.velocity.y > 0.01f;
  }

  ///  If GameObject is falling. Is independent from animation
  protected bool IsFalling()
  {
    return rb.velocity.y < -0.1f;
  }

  /// Sets the passed `IsRunning()` parameter in the Animator tab
  protected void SetRunAnimationParam(bool value)
  {
    this.anim.SetBool(Running, value);
  }

  /// Sets the passed `IsJumping()` parameter in the Animator tab
  protected void SetJumpAnimationParam(bool value)
  {
    this.anim.SetBool(Jumping, value);
  }

  /// Sets the passed `IsFalling()` parameter in the Animator tab
  protected void SetFallAnimationParam(bool value)
  {
    this.anim.SetBool(Falling, value);
  }
  
  /// Called when this object collides with something
  private void OnCollisionEnter2D()
  {
    this.IsGrounded = true;
  }

  /// Called when this object leaves a collider
  private void OnCollisionExit2D()
  {
    this.IsGrounded = false;
  }
}
