using UnityEngine.Assertions;
using UnityEngine;

public sealed class BackgroundManager : MonoBehaviour
{
  // How many extra backgrounds to make
  public int numAdditionalBgs = 1;

  [Header("Background Prefab")]
  [SerializeField] private GameObject bg;

  [Header("Spritemask")]
  [SerializeField] private SpriteMask sm;

  // How fast the screen moves
  [SerializeField] private float scrollVelocity = 1.5f;

  [Header("Debug")]
  [SerializeField] private bool shouldMove;

  public float GetScrollVelocity() { return scrollVelocity; }

  public void SetScrollVelocity(float sv)
  {
    scrollVelocity = sv;
    Debug.Log("New scroll velocity: " + scrollVelocity);
  }

  public bool ShouldMove() { return shouldMove; }

  void Start()
  {
    Assert.IsTrue(numAdditionalBgs > 0);
    GameObject[] duplicates = MakeDuplicates(numAdditionalBgs);
    float length = sm.bounds.size.x;
    PositionBackgrounds(duplicates, length);
  }

  // Makes num number of Background duplicates
  GameObject[] MakeDuplicates(int num)
  {
    GameObject[] dups = new GameObject[num];
    for (int i = 0; i < num; i++)
    {
      dups[i] = Instantiate(bg);
      dups[i].name = $"Background Copy ({i + 1})";
    }

    return dups;
  }

  // Position each background on the screen
  void PositionBackgrounds(GameObject[] dups, float length)
  {
    for (int i = 1; i <= dups.Length; i++)
      dups[i - 1].transform.position = new Vector3(bg.transform.position.x + (length * i), bg.transform.position.y, bg.transform.position.z);
  }


}
