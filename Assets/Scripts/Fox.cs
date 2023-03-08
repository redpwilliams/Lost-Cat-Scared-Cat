using UnityEngine;

/// <summary>
/// Represents all foxes. 
/// All sub-instances either jump or don't jump

///</summary>
public abstract class Fox : Character
{
  /// <summary>if false, it's a lunging fox</summary>
  protected bool isJumpingFox;

  
  protected virtual void Start()
  {
    // Determine if the Fox will jump
    isJumpingFox = Random.Range(0, 2) == 1;
  }

  /// <summary>
  /// A Fox's attack sequence
  /// </summary>
  protected abstract void Attack();
}