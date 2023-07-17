using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;

public class FoxSpawner : MonoBehaviour
{
  
  private readonly float startX = 4.5f;
  private readonly float startY = -0.875f;
  private Transform trans;
  
  [SerializeField] private GameObject[] foxPrefabs;

  [SerializeField] private float skulkSpawnInterval = 5f;
  [SerializeField] private float foxSpawnInterval = 3f;

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
      // Set interval in between skulk spawns
      yield return new WaitForSeconds(this.skulkSpawnInterval);
      
      // Create skulk
      Skulk skulk = new Skulk(4, this.foxPrefabs);
      IEnumerable<GameObject> foxes = skulk.GetSkulk();

      // Spawn each Fox in the skulk
      foreach (GameObject fox in foxes)
      {
        Instantiate(fox, this.trans.position, this.trans.rotation);
        yield return new WaitForSeconds(this.foxSpawnInterval);
      }
    }
  }
}


public class Skulk
{
  private static readonly int MaxSize = 5;
  private static readonly int MinSize = 3;
  
  private readonly GameObject[] skulk;
  private readonly GameObject[] foxPrefabs;

  public Skulk(GameObject[] foxPrefabs) : this((Range(MinSize, MaxSize)), foxPrefabs) { }

  public Skulk(int size, GameObject[] foxPrefabs)
  {
    
    // Include Fox prefabs
    this.foxPrefabs = foxPrefabs;
    
    // Init skulk array
    skulk = new GameObject[size];
    
    // Fill in skulk with 
    for (int i = 0; i < this.skulk.Length; i++)
    {
      this.skulk[i] = ChooseFox();
    }
  }

  public IEnumerable<GameObject> GetSkulk()
  {
    return this.skulk;
  }
  
  private GameObject ChooseFox()
  {
    // Number of Types of Foxes    
    int numFoxes = this.foxPrefabs.Length;
    int choice = (int)(value * numFoxes);

    return this.foxPrefabs[choice];
  }
}