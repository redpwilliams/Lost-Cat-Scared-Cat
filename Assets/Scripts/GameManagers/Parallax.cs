using UnityEngine;

public class Parallax : MonoBehaviour
{

  // The sprite's length
  private float spriteLength;

  // How much parallax to add
  [SerializeField] private float parallaxEffect;
  [SerializeField] private BackgroundManager bgm;

  // If the sprite is visible/off screen
  private bool isVisible;

  // Rigidbody2D to set velocity
  private Rigidbody2D rb;

  void Start()
  {
    spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;
    bgm = bgm.GetComponent<BackgroundManager>();
    isVisible = true;
    rb = GetComponent<Rigidbody2D>();
  }

  void FixedUpdate()
  {
    float scrollVelocity = bgm.GetScrollVelocity();
    // Set apparent screen movement speed
    rb.velocity = Vector2.left * parallaxEffect * scrollVelocity;

    // If object is to the left of the camera & out-of-bounds
    if (!isVisible && scrollVelocity > 0 && transform.position.x < Camera.main.transform.position.x)
    {
      // Move sprite 2 positions over (to cover for BackgroundManager's additional sprites)
      transform.position = new Vector3(transform.position.x + 2 * spriteLength, transform.position.y, transform.position.z);
    }

  }

  void OnBecameInvisible()
  {
    isVisible = false;
  }

  void OnBecameVisible()
  {
    isVisible = true;
  }

}
