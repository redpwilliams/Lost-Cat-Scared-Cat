using System.Collections;
using UnityEngine;
using static UnityEngine.Random;

/// <summary>
/// Spawns foxes at specific intervals
/// </summary>
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

    /// <summary>
    /// Starts the spawning of foxes
    /// </summary>
    private void Start()
    {
        _trans = GetComponent<Transform>();
        _trans.localPosition = new Vector3(StartX, StartY,
            _trans.position.z);

        StartCoroutine(SpawnFoxes());
    }

    /// <summary>
    /// Spawns foxes at specific intervals using skulks
    /// </summary>
    /// <returns>An enumerator for controlling the spawning loop</returns>
    private IEnumerator SpawnFoxes()
    {
        while (!_isGameOver) // TODO Change when game is over
        {
            // Set interval in between skulk spawns
            yield return new WaitForSeconds(_skulkSpawnInterval);

            // Create skulk
            Skulk skulk = new Skulk(_skulkSize, _foxPrefabs);

            // Spawn each Fox in the skulk
            for (int i = 0; i < skulk.Size; i++)
            {
                Instantiate(skulk[i], _trans.position, _trans.rotation);
                yield return new WaitForSeconds(_foxSpawnInterval);
            }
        }
    }
}


/// <summary>
/// Represents a group of foxes to be spawned
/// </summary>
public sealed class Skulk
{
    private const int MaxSize = 5;
    private const int MinSize = 3;

    /// <summary>
    /// Gets the size of the skulk
    /// </summary>
    public int Size { get; }

    private readonly GameObject[] _skulk;
    private readonly GameObject[] _foxPrefabs;

    /// <summary>
    /// Initializes a new instance of the <see cref="Skulk"/> class
    /// with random size.
    /// </summary>
    public Skulk(GameObject[] foxPrefabs) : this(Range(MinSize, MaxSize),
        foxPrefabs) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Skulk"/> class
    /// with specified size.
    /// </summary>
    public Skulk(int size, GameObject[] foxPrefabs)
    {
        // Include Fox prefabs
        _foxPrefabs = foxPrefabs;

        // Init skulk array
        _skulk = new GameObject[size];
        Size = size;

        // Fill in skulk with 
        for (int i = 0; i < _skulk.Length; i++)
        {
            _skulk[i] = ChooseFox();
        }
    }

    /// <summary>
    /// Gets the fox prefab at the specified index.
    /// </summary>
    /// <param name="index">The index of the fox prefab.</param>
    /// <returns>The selected fox prefab.</returns>
    public GameObject this[int index] => _skulk[index];

    private GameObject ChooseFox()
    {
        int choice = Range(0, _foxPrefabs.Length);
        return _foxPrefabs[choice];
    }
}