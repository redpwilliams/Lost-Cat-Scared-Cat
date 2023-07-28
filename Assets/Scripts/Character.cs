using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(SpriteLibrary))]
[RequireComponent(typeof(SpriteResolver))]
public abstract class Character : MonoBehaviour
{
  protected Rigidbody2D rb;
  private Animator anim;
  private new Transform transform;

  protected bool IsGrounded { get; private set; }

  protected bool IsVisiblyJumping { get; set; }

  private bool isVisiblyRunning;

  protected bool IsVisiblyRunning { get; set; }

  protected bool HasInputJump { get; set; }

  private static readonly int Running = Animator.StringToHash("IsRunning");
  private static readonly int Jumping = Animator.StringToHash("IsJumping");
  private static readonly int Falling = Animator.StringToHash("IsFalling");
  private static readonly int Crouching = Animator.StringToHash("IsCrouching");
  private static readonly int Dashing = Animator.StringToHash("IsDashing");
  
  protected virtual void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    transform = GetComponent<Transform>();

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

  /// If GameObject is falling. Is independent from animation
  protected bool IsFalling()
  {
    return rb.velocity.y < -0.1f;
  }

  /// Sets GameObject's speed as idle aligned with the parallax background
  protected void SetSpeedAsIdle()
  {
    float idleSpeed = BackgroundManager.bgm.GetScrollVelocity() * 1.1f;

    Vector3 currentPosition = transform.position;
    this.transform.position = new Vector3(
      currentPosition.x - idleSpeed * Time.deltaTime, currentPosition.y,
      currentPosition.z);
  }

  [UsedImplicitly]
  protected abstract void HandleJumpAnimationEvent();
  
  /// Sets the passed `IsRunning` parameter in the Animator tab
  protected void SetRunAnimationParam(bool value)
  {
    this.anim.SetBool(Running, value);
  }

  /// Sets the passed `IsJumping` parameter in the Animator tab
  protected void SetJumpAnimationParam(bool value)
  {
    this.anim.SetBool(Jumping, value);
  }

  /// Sets the passed `IsFalling` parameter in the Animator tab
  protected void SetFallAnimationParam(bool value)
  {
    this.anim.SetBool(Falling, value);
  }
  
  /// Sets the passed `IsCrouching` parameter in the Animator tab
  protected void SetCrouchingAnimationParam(bool value)
  {
    this.anim.SetBool(Crouching, value);
  }

  /// Sets the passed `IsDashing` parameter in the Animator tab
  protected void SetDashingAnimationParam(bool value)
  {
    this.anim.SetBool(Dashing, value);
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
