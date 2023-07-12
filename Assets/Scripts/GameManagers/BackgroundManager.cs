using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public sealed class BackgroundManager : MonoBehaviour
{
  // How many extra backgrounds to make
  [FormerlySerializedAs("numAdditionalBgs")] public int backgroundCount = 1;

  [Header("Background Prefab")]
  [SerializeField] private GameObject bg;

  [Header("Sprite Mask")]
  [SerializeField] private SpriteMask sm;

  // How fast the screen moves
  [SerializeField] private float scrollVelocity = 1.5f;

  [Header("Debug")]
  [SerializeField] private bool shouldMove;

  public float GetScrollVelocity() { return scrollVelocity; }

  // Will probably use when accelerating screen
  public void SetScrollVelocity(float sv)
  {
    scrollVelocity = sv;
    Debug.Log("New scroll velocity: " + scrollVelocity);
  }

  public bool ShouldMove() { return shouldMove; }

  private void Start()
  {
    Assert.IsTrue(this.backgroundCount > 0);
    GameObject[] duplicates = MakeDuplicates(this.backgroundCount);
    float length = sm.bounds.size.x;
    PositionBackgrounds(duplicates, length);
  }

  // Makes num number of Background duplicates
  private GameObject[] MakeDuplicates(int num)
  {
    GameObject[] duplicates = new GameObject[num];
    for (int i = 0; i < num; i++)
    {
      duplicates[i] = Instantiate(bg);
      duplicates[i].name = $"Background Copy ({i + 1})";
    }

    return duplicates;
  }

  // Position each background on the screen
  private void PositionBackgrounds(IReadOnlyList<GameObject> duplicates, float length)
  {
    for (int i = 1; i <= duplicates.Count; i++)
    {
      Vector3 position = bg.transform.position;
      duplicates[i - 1].transform.position = new Vector3(position.x + (length * i), position.y, position.z);
    }
  }
}
