using UnityEngine;

public class Player : Character
{
  public static float PLAYER_X_POS = -1.75f;

  protected override void Start()
  {
    base.Start();
    gameObject.transform.position = new Vector3(PLAYER_X_POS, this.transform.position.y, this.transform.position.z);
  }
  protected override void FixedUpdate()
  {
    if (Input.GetKey(KeyCode.Space) && state == MovementState.Running)
      rb.velocity = Vector2.up * jumpVelocity;
  }

}
