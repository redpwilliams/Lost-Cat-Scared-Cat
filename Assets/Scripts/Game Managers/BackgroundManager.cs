using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class BackgroundManager : MonoBehaviour
{
  /// Singleton Instance
  public static BackgroundManager bgm;
  
  // How many extra backgrounds to make
  public const int BackgroundCount = 1;

  [Header("Background Prefab")]
  [SerializeField] private GameObject _bgSet;

  /// SpriteMask component
  private SpriteMask _sm;

  /// How fast the screen moves
  [field: SerializeField]
  public float ScrollVelocity { get; private set; } = 1f;
  private float _scrollVelocity;

  private void Awake()
  {
    // Craft singleton instance
    if (bgm != null)
    {
      Destroy(bgm);
      return;
    }

    bgm = this;
    _sm = GetComponent<SpriteMask>();
    _scrollVelocity = ScrollVelocity;
    ScrollVelocity = 0;
  }

  private void OnEnable()
  {
    EventManager.Events.OnPlayStart += InitScroll;
  }

  private void OnDisable()
  {
    EventManager.Events.OnPlayStart -= InitScroll;
  }

  private void Start()
  {
    Assert.IsTrue(BackgroundCount > 0);
    GameObject[] duplicates = MakeDuplicates(BackgroundCount);
    float length = _sm.bounds.size.x;
    PositionBackgrounds(duplicates, length);
  }

  /// Makes num number of Background duplicates
  private GameObject[] MakeDuplicates(int num)
  {
    GameObject[] duplicates = new GameObject[num];
    for (int i = 0; i < num; i++)
    {
      duplicates[i] = Instantiate(_bgSet);
      duplicates[i].name = $"Background Copy ({i + 1})";
    }

    return duplicates;
  }

  /// Position each background on the screen
  private void PositionBackgrounds(IReadOnlyList<GameObject> duplicates, float length)
  {
    for (int i = 1; i <= duplicates.Count; i++)
    {
      Vector3 position = _bgSet.transform.position;
      duplicates[i - 1].transform.position = new Vector3(position.x + (length * i), position.y, position.z);
    }
  }

  /// Enables background scroll by allowing
  private void InitScroll()
  {
    ScrollVelocity = _scrollVelocity;
  }
}
