using UnityEngine;

public class FoxSpawner : MonoBehaviour
{
  
  public ForegroundFox FFox;
  public float _spawnRate = 2f;
  private float _timer = 0f;
  private float _startX = -4.5f;
  private float _startY = -0.875f;
  private Transform _transform;

  void Start()
  {
    _transform = GetComponent<Transform>();
    _transform.localPosition = new Vector3(_startX, _startY, _transform.position.z);
  }

  void Update()
  {
    if (_timer < _spawnRate)
    {
      _timer += Time.deltaTime;
      return;
    }
    FFox = ChooseFox();
    Instantiate<Fox>(FFox, _transform.position, _transform.rotation);
    _timer = 0;
  }

  ForegroundFox ChooseFox()
  {
    return FFox;
  }
}