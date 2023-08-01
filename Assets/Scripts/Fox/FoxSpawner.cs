using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;

public class FoxSpawner : MonoBehaviour
{
    private readonly float _startX = 4.5f;
    private readonly float _startY = -0.875f;
    private Transform _trans;

    [SerializeField] private GameObject[] _foxPrefabs;

    [SerializeField] private float _skulkSpawnInterval = 5f;
    [SerializeField] private float _foxSpawnInterval = 3f;

    [Header("Debug")] [SerializeField] private bool _shouldSpawn;

    private void Start()
    {
        this._trans = GetComponent<Transform>();
        this._trans.localPosition = new Vector3(this._startX, this._startY,
            this._trans.position.z);

        StartCoroutine(SpawnFoxes());
    }

    private IEnumerator SpawnFoxes()
    {
        while (this._shouldSpawn)
        {
            // Set interval in between skulk spawns
            yield return new WaitForSeconds(this._skulkSpawnInterval);

            // Create skulk
            Skulk skulk = new Skulk(4, this._foxPrefabs);
            IEnumerable<GameObject> foxes = skulk.GetSkulk();

            // Spawn each Fox in the skulk
            foreach (GameObject fox in foxes)
            {
                Instantiate(fox, this._trans.position, this._trans.rotation);
                yield return new WaitForSeconds(this._foxSpawnInterval);
            }
        }
    }
}


public class Skulk
{
    private static readonly int MaxSize = 5;
    private static readonly int MinSize = 3;

    private readonly GameObject[] _skulk;
    private readonly GameObject[] _foxPrefabs;

    public Skulk(GameObject[] foxPrefabs) : this((Range(MinSize, MaxSize)),
        foxPrefabs) {}

    public Skulk(int size, GameObject[] foxPrefabs)
    {
        // Include Fox prefabs
        this._foxPrefabs = foxPrefabs;

        // Init skulk array
        _skulk = new GameObject[size];

        // Fill in skulk with 
        for (int i = 0; i < this._skulk.Length; i++)
        {
            this._skulk[i] = ChooseFox();
        }
    }

    public IEnumerable<GameObject> GetSkulk()
    {
        return this._skulk;
    }

  private GameObject ChooseFox()
  {
      int choice = Range(0, this._foxPrefabs.Length);
      return this._foxPrefabs[choice];
  }
}