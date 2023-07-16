using System;
using JetBrains.Annotations;
using UnityEngine;

public abstract class Fox : Character
{
  /// Default Fox velocity
  [SerializeField] private float runSpeed = -3f;

  /// Fox destruction point
  private readonly float deadZone = -4.5f;

  /// True if the Fox has initiated its `Attack()` method
  protected bool hasAttacked = false;

  /// The amount of space between the Fox and Player to initiate Fox attack
  private readonly float spaceBeforeAttack = 1.5f;
  // REVIEW - I could change this into an array to accommodate for sitting foxes
  // Each value in the array would dictate the spacing before each action of a sitting fox

  protected void Start()
  {
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

  /// A Fox's attack sequence
  protected abstract void Attack();

  /// Returns true if the Fox is outside the dead zone
  private static bool IsOffscreen(float currPos, float deadZone)
  {
    return currPos < deadZone;
  }

  /// Returns true if the Fox is at the correct distance to Attack
  private static bool IsInPosition(float distance, float spacing)
  {
    return distance < spacing;
  }

  private void HandleAttackPosition()
  {
    throw new NotImplementedException();
  }
  
  [UsedImplicitly] private void HandleAttackAction(float jumpForce)
  {
    // Used in White Fox animation event at start of Jump clip
    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // TODO - Export 3f to variable
    IsVisiblyJumping = true;
    HasInputJump = false;
  }

}