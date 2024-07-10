using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.Random;

/// <summary>
/// Spawns foxes at specific intervals
/// </summary>
public sealed class FoxSpawner : MonoBehaviour
{
    private Transform _trans;
    private const float StartX = 4.5f;
    private const float StartY = -0.875f;
    public const float MinAttackGap = 0.75f;
    public const float MaxAttackGap = 1.5f;

    [SerializeField] private GameObject[] _foxPrefabs;

    [SerializeField] private float _foxSpawnInterval = 3f;
    [SerializeField] private float _skulkSpawnInterval = 5f;
    [SerializeField] private float _tutorialSkulkSpawnInterval = 2f;
    [SerializeField] private bool _drawGizmos;

    private IEnumerator _spawnMain;
    private IEnumerator _spawnTutorial;

    private bool _isFirstTime;
    private bool _isFinishedSpawning;
    private bool _isGameOver;

    private void OnDrawGizmos()
    {
        if (!_drawGizmos) return;
        Gizmos.DrawSphere(new Vector3(MinAttackGap, -0.8f, 0f), 0.1f);
        Gizmos.DrawSphere(new Vector3(MaxAttackGap, -0.8f, 0f), 0.1f);
    }

    private void OnEnable()
    {
        EventManager.Events.OnPlayStart += BeginSpawnFoxes;
        EventManager.Events.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        EventManager.Events.OnPlayStart -= BeginSpawnFoxes;
        EventManager.Events.OnGameOver -= HandleGameOver;
    }

    private void Awake()
    {
        _trans = GetComponent<Transform>();
        _trans.localPosition = new Vector3(StartX, StartY, _trans.position.z);
        
        _spawnMain = SpawnFoxesMain();
        _spawnTutorial = SpawnFoxesTutorial();

        _isFirstTime = SaveSystem.LoadPreferences().IsFirstTime;
    }

    /// <summary>
    /// Starts the spawning of foxes
    /// </summary>
    private void BeginSpawnFoxes()
    {
        StartCoroutine(_isFirstTime ? _spawnTutorial : _spawnMain);
    }

    /// <summary>
    /// Spawns foxes at specific intervals using skulks
    /// </summary>
    /// <returns>An enumerator for controlling the spawning loop</returns>
    private IEnumerator SpawnFoxesMain()
    {
        // Start main spawn loop
        while (!_isGameOver) 
        {
            // Set interval in between skulk spawns
            yield return new WaitForSeconds(_skulkSpawnInterval);

            // Create skulk
            Skulk skulk = new Skulk(_foxPrefabs);

            // Spawn each Fox in the skulk
            for (int i = 0; i < skulk.Size; i++)
            {
                Instantiate(skulk[i], _trans.position, _trans.rotation);
                yield return new WaitForSeconds(_foxSpawnInterval);
            }
        }
    }

    /// <summary>
    /// Spawns specific foxes for a tutorial section
    /// </summary>
    /// <returns>An enumerator for controlling the spawning loop</returns>
    private IEnumerator SpawnFoxesTutorial()
    {
        int tutorialSkulkSize = 1;
        GameObject[] tempFoxes = new GameObject[tutorialSkulkSize];
        
        // Red Fox Skulk
        Array.Fill(tempFoxes, _foxPrefabs[0]);
        yield return InstantiateTutorialSkulk(new Skulk(tempFoxes.Length, tempFoxes));
        
        // Brown Fox Skulk
        Array.Fill(tempFoxes, _foxPrefabs[1]);
        yield return InstantiateTutorialSkulk(new Skulk(tempFoxes.Length, tempFoxes));
        
        // Gray Fox Skulk
        Array.Fill(tempFoxes, _foxPrefabs[2]);
        yield return InstantiateTutorialSkulk(new Skulk(tempFoxes.Length, tempFoxes));

        // Additional hang time to keep tutorial text on screen
        yield return new WaitForSeconds(_tutorialSkulkSpawnInterval);
        
        EventManager.Events.CompleteTutorialSkulks();
        StartCoroutine(SpawnFoxesMain());
    }

    private IEnumerator InstantiateTutorialSkulk(Skulk skulk)
    {
        yield return new WaitForSeconds(_tutorialSkulkSpawnInterval);
        
        for (int i = 0; i < skulk.Size; i++)
        {
            Instantiate(skulk[i], _trans.position, _trans.rotation);
            yield return new WaitForSeconds(_foxSpawnInterval);
        }
    }

    private void HandleGameOver()
    {
        StopCoroutine(_spawnMain);
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