using UnityEngine;
using System.Collections;

public class FoxSpawner : MonoBehaviour
{
  
  private readonly float startX = 4.5f;
  private readonly float startY = -0.875f;
  private Transform trans;
  
  [Header("Fox Prefabs")]
  [SerializeField] private GameObject redFox;
  [SerializeField] private GameObject whiteFox;

  [Header("Debug")]
  [SerializeField] private bool shouldSpawn;

  private void Start()
  {
    this.trans = GetComponent<Transform>();
    this.trans.localPosition = new Vector3(this.startX, this.startY, this.trans.position.z);

    StartCoroutine(SpawnFoxes());
  }

  private IEnumerator SpawnFoxes()
  {
    while (this.shouldSpawn)
    {
      InstantiateRandomFox();
      yield return new WaitForSeconds(2);
    }
  }

  private void InstantiateRandomFox()
  {
    GameObject foxClone;

    if (Random.value < 0.5f)
    {
      // Create RunningFox
      foxClone = Instantiate(redFox, this.trans.position, this.trans.rotation);
      foxClone.name = "Red Fox (Clone)";
    }
    else  
    {
      foxClone = Instantiate(whiteFox, this.trans.position, this.trans.rotation);
      foxClone.name = "White Fox (Clone)";
    }
  }
}