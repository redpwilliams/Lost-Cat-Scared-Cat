using UnityEngine;

public class Parallax : MonoBehaviour
{

  // The sprite's length
  private float spriteLength;

  // How much parallax to add
  [SerializeField] private float parallaxEffect;
  [SerializeField] private BackgroundManager bgm;
  [SerializeField] private new Camera camera;
  
  private new Transform transform;
  private int numBackgrounds;

  // If the sprite is visible/off screen
  public bool isVisible;

  public float ParallaxBackgroundSpeed { get; set; }

  private void Start()
  {
    spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;
    transform = GetComponent<Transform>();
    numBackgrounds = this.bgm.backgroundCount;
    isVisible = true;
    ParallaxBackgroundSpeed =
      this.bgm.GetScrollVelocity() * this.parallaxEffect;
  }

  private void Update()
  {
    // Determine new background position based on its parallax value
    Vector3 currentPosition = transform.position;

    // Set its new position
    this.transform.position =
      new Vector3(currentPosition.x - ParallaxBackgroundSpeed * Time.deltaTime,
        currentPosition.y, currentPosition.z);
    
    // If object is visible in the camera & not out-of-bounds
    if (this.isVisible || !(transform.position.x < camera.transform.position.x)) return;
    // Move sprite the number of background sets + 1 positions over
    // (to cover for BackgroundManager's possible additional sprites)
    currentPosition = this.transform.position;
    
   transform.position = new Vector3(currentPosition.x + (this.numBackgrounds + 1) * this.spriteLength,
      currentPosition.y, currentPosition.z);

  }

  private void OnBecameInvisible()
  {
    isVisible = false;
  }

  private void OnBecameVisible()
  {
    isVisible = true;
  }

  public bool Test()
  {
    return true;
  }

}
