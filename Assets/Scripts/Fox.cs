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
    isJumpingFox = Random.Range(0, 2) == 1;
  }

  protected abstract void Attack();
}