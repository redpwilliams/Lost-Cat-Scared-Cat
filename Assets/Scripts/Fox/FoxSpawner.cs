using UnityEngine;
using System.Collections;

public class FoxSpawner : MonoBehaviour
{
  
  private float _startX = 4.5f;
  private float _startY = -0.875f;
  private Transform _transform;

  
  [SerializeField] private GameObject rf;
  [SerializeField] private GameObject sf;

  void Start()
  {
    _transform = GetComponent<Transform>();
    _transform.localPosition = new Vector3(_startX, _startY, _transform.position.z);
    StartCoroutine(SpawnFoxes());
  }

  private IEnumerator SpawnFoxes()
  {
    while (true) // TODO - Needs to listen to a Game Status Even, only true if game is playing/not paused
    {
      InstantiateRandomFox();
      yield return new WaitForSeconds(2);
    }
  }

  

  void InstantiateRandomFox()
  {
    if (Random.value < 0.5f)
    {
      // Create RunningFox
      Instantiate<GameObject>(rf, _transform.position, _transform.rotation);
    return;
    } 
      Instantiate<GameObject>(sf, _transform.position, _transform.rotation);
  }

}