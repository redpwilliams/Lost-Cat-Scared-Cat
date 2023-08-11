using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;

public sealed class FoxSpawner : MonoBehaviour
{
    private const float StartX = 4.5f;
    private const float StartY = -0.875f;
    private Transform _trans;

    public const float MinAttackGap = 0.75f;
    public const float MaxAttackGap = 1.5f;

    [SerializeField] private GameObject[] _foxPrefabs;

    [SerializeField] private float _skulkSpawnInterval = 5f;
    [SerializeField] private float _foxSpawnInterval = 3f;
    [SerializeField] private int _skulkSize = 1;

    [SerializeField] private bool _drawGizmos;

    private bool _isGameOver;
    
    private void OnDrawGizmos()
    {
        if (!_drawGizmos) return;
        Gizmos.DrawSphere(new Vector3(MinAttackGap, -0.8f, 0f), 0.1f);
        Gizmos.DrawSphere(new Vector3(MaxAttackGap, -0.8f, 0f), 0.1f);
    }

    private void Start()
    {
        _trans = GetComponent<Transform>();
        _trans.localPosition = new Vector3(StartX, StartY,
            _trans.position.z);

        StartCoroutine(SpawnFoxes());
    }

    private IEnumerator SpawnFoxes()
    {
        while (!_isGameOver) // TODO Change when game is over
        {
            // Set interval in between skulk spawns
            yield return new WaitForSeconds(_skulkSpawnInterval);

            // Create skulk
            Skulk skulk = new Skulk(_skulkSize, _foxPrefabs);
            IEnumerable<GameObject> foxes = skulk.GetSkulk();

            // Spawn each Fox in the skulk
            foreach (GameObject fox in foxes)
            {
                Instantiate(fox, _trans.position, _trans.rotation);
                yield return new WaitForSeconds(_foxSpawnInterval);
            }
        }
    }
}


public class Skulk
{
    private const int MaxSize = 5;
    private const int MinSize = 3;

    private readonly GameObject[] _skulk;
    private readonly GameObject[] _foxPrefabs;

    public Skulk(GameObject[] foxPrefabs) : this(Range(MinSize, MaxSize),
        foxPrefabs) { }


    public Skulk(int size, GameObject[] foxPrefabs)
    {
        // Include Fox prefabs
        _foxPrefabs = foxPrefabs;

        // Init skulk array
        _skulk = new GameObject[size];

        // Fill in skulk with 
        for (int i = 0; i < _skulk.Length; i++)
        {
            _skulk[i] = ChooseFox();
        }
    }

    public IEnumerable<GameObject> GetSkulk()
    {
        return _skulk;
    }

  private GameObject ChooseFox()
  {
      int choice = Range(0, _foxPrefabs.Length);
      return _foxPrefabs[choice];
  }
}