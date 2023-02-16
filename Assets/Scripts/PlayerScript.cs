using UnityEngine;

public class PlayerScript : MonoBehaviour
{
  private Rigidbody2D rb;
  private Animator anim;

  [SerializeField] private float jumpVelocity = 3f;
  private bool isGrounded;

  private enum MovementState { idle, running, jumping, falling, nil }
  private MovementState state;

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    state = MovementState.running;
    isGrounded = true;
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    if (Input.GetKey(KeyCode.Space) && state == MovementState.running)
      rb.velocity = Vector2.up * jumpVelocity;
  }

  void Update()
  {
    if (isJumping())
      state = MovementState.jumping;
    else if (isFalling())
      state = MovementState.falling;
    else if (isRunning())
      state = MovementState.running;
    else
      state = MovementState.nil; // Fixes jump between jump and fall animation
    UpdateAnimationState();
  }

  void OnCollisionEnter2D(Collision2D col)
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

  void OnCollisionExit2D(Collision2D col)
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

  // Updates the animation state in the Animator
  private void UpdateAnimationState()
  {
    anim.SetInteger("state", (int)state);
  }

  private bool isRunning()
  {
    return isGrounded;
  }

  // If GameObject is jumping. Is independent from animation
  private bool isJumping()
  {
    return rb.velocity.y > 0.01f;
  }

  // If GameObject is falling. Is independent from animation
  private bool isFalling()
  {
    return rb.velocity.y < -0.1f;
  }

}
