using UnityEngine;

public class Player : Character
{
  public static float PLAYER_X_POS = -1.75f;

  /// <summary>Upward velocity magnitude use for jump speed/height </summary>
  [SerializeField] private float jumpVelocity;

  protected override void Awake()
  {
    base.Awake();
    jumpVelocity = 3f;
  }

  protected override void Start()
  {
    base.Start();
    state = MovementState.Idle;

    // Put Player on specific location on screen
    gameObject.transform.position = new Vector3(PLAYER_X_POS, this.transform.position.y, this.transform.position.z);
  }

  protected override void FixedUpdate()
  {
    // Handle Jump input
    if (Input.GetKey(KeyCode.Space) && state == MovementState.Running)
      rb.velocity = Vector2.up * jumpVelocity;
  }

  protected override bool IsRunning()
  {
    return (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow));
  }

}
