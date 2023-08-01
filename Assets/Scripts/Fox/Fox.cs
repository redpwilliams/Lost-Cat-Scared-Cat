using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Fox : Character
{
  /// Default Fox velocity
  [field: SerializeField]
  protected float RunSpeed { get; set; } = 3f;

  /// Fox destruction point
  private const float DeadZone = -4.5f;

  /// True if the Fox has initiated its `Attack()` method
  protected bool HasAttacked { get; set; }
  
  private void OnValidate()
  {
    RunSpeed = Mathf.Abs(RunSpeed);
  }

  protected void Start()
  {
    // Flip sprite horizontally
    Transform rbTransform = this.rb.transform;
    Vector3 localScale = rbTransform.localScale;
    localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
    rbTransform.localScale = localScale;
  }

  protected virtual void Update()
  {
    // Destroy on off-screen
    if (IsOffscreen(transform.position.x, DeadZone)) 
    {
      Destroy(gameObject);
      return;
    }

    // Update animation
    SetAnimationParams();
  }

  protected void FixedUpdate()
  {
    HandleMovement();
    
    // Distance between Fox and Player's position && isRunning
    float currentPosition = Mathf.Abs(transform.position.x);
    if (HasAttacked || !IsInPosition(currentPosition,
          GetRandomAttackGap(FoxSpawner.MinAttackGap, FoxSpawner.MaxAttackGap)))
      return;
    
    Attack();
  }

  protected abstract void HandleMovement();
  
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

  /// Gets a random attack position within the Min and Max range
  private static float GetRandomAttackGap(float min, float max)
  {
    return Random.Range(min, max);
  }
  
  protected abstract void SetAnimationParams();

}