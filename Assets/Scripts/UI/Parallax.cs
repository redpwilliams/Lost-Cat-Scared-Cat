using UnityEngine;

public sealed class Parallax : MonoBehaviour
{
  /// The sprite's length
  private float _spriteLength;

  /// How much parallax to add / Parallax coefficient
  [SerializeField] private float _parallaxEffect;

  private Camera _cam;
  private Transform _trans;
  
  /// The number of backgrounds in sequence, left to right
  private int _numBackgrounds;

  /// If the sprite is visible/off screen
  private bool _isVisible;

  /// The calculated background speed.
  /// Product of the BGM scroll velocity and the parallax effect
  private float ParallaxBackgroundSpeed { get; set; }

  private void Awake()
  {
    _spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;
    _trans = transform;
    _cam = Camera.main;
    _isVisible = true;
  }

  private void Start()
  {
    _numBackgrounds = BackgroundManager.BackgroundCount;
  }

  private void Update()
  {
    // Determine new background position based on its parallax value
    Vector3 currentPosition = transform.position;

    ParallaxBackgroundSpeed = BackgroundManager.bgm.ScrollVelocity * _parallaxEffect;
    
    // Set its new position
    _trans.position =
      new Vector3(currentPosition.x - ParallaxBackgroundSpeed * Time.deltaTime,
        currentPosition.y, currentPosition.z);
    
    // If object is visible in the camera & not out-of-bounds
    if (_isVisible || !(transform.position.x < _cam.transform.position.x))
     return;
    
    // Move sprite the number of background sets + 1 positions over
    // (to cover for BackgroundManager's possible additional sprites)
    currentPosition = _trans.position; 
    
    // And set
    _trans.position = new Vector3(
        currentPosition.x + (_numBackgrounds + 1) * _spriteLength,
        currentPosition.y, currentPosition.z);
  }

  private void OnBecameInvisible()
  {
    _isVisible = false;
  }

  private void OnBecameVisible()
  {
    _isVisible = true;
  }
}
