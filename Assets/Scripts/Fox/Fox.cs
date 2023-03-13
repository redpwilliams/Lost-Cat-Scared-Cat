using UnityEngine;

/// <summary>
/// Represents all foxes. 
/// All sub-instances either jump or don't jump

///</summary>
public abstract class Fox : Character
{
  /// <summary>if false, it's a lunging fox</summary>
  protected bool isJumpingFox;

  /// <summary> The Fox's horizontal velocity </summary>
  [SerializeField] protected float horizontalVelocity = 2f;

  protected override void Start()
  {
    base.Start();
    // Determine if the Fox will jump
    isJumpingFox = Random.Range(0, 2) == 1;
  }

  protected override void Update()
  {
    base.Update(); // ForegroundFox and BackgroundFox need to call Update() of Character
  }

  protected override void FixedUpdate()
  {
    float verticalVelocity = 0;
    if (Input.GetKey(KeyCode.F) && state == MovementState.running)
    {
      verticalVelocity = jumpVelocity;
    }
    rb.velocity = new Vector2(horizontalVelocity, verticalVelocity);
  }

  /// <summary>
  /// A Fox's attack sequence
  /// </summary>
  protected abstract void Attack();

  protected override void OnCollisionEnter2D(Collision2D col)
  {
    isGrounded = true;
    switch (LayerMask.LayerToName(col.gameObject.layer))
    {
      case "Ground":
        // Do something
        break;

      default:
        // Do something
        break;
    }
  }

  protected override void OnCollisionExit2D(Collision2D col)
  {
    isGrounded = false;
    switch (LayerMask.LayerToName(col.gameObject.layer))
    {
      case "Ground":
        // Do something
        break;

      default:
        // Do something
        break;
    }
  }
}