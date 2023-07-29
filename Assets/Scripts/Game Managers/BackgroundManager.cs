using UnityEngine;

public sealed class BackgroundManager : Singleton<BackgroundManager>
{
  
  // How many extra backgrounds to make
  public const uint BackgroundCount = 1;

  /// How fast the screen moves
  [SerializeField] private float scrollVelocity = 1.5f;

  /// BackgroundLayer represents each individual layer
  [SerializeField] private BackgroundLayer[] layers;

  private Transform trans;
  private float length;
  
  // BackgroundGroups include Forest, Snowy, Woody, etc.

  // Current Environment to render

  protected override void Awake()
  {
    base.Awake();
    trans = transform;
    length = GetComponent<SpriteMask>().bounds.size.x;
  }

  private void Start()
  {
    // Set up self and duplicate adjacent layers
    // (Backwards because it looks nice in the editor)
    for (int i = this.layers.Length - 1; i >= 0; i--)
    {
      MakeDuplicates(layers[i], BackgroundCount);
    }
  }

  /// Makes num number of Background duplicates
  private void MakeDuplicates(BackgroundLayer layer, uint numDuplicates)
  {
    for (int i = 0; i < numDuplicates; i++)
    {
      GameObject go = layer.Initialize();
      go.name = $"Background Copy ({i + 1})";
      go.transform.Translate(Vector3.right * length * (i + 1));
    }
  }

  public float GetScrollVelocity() { return scrollVelocity; }
  
}
