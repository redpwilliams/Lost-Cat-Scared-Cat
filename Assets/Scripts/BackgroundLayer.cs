using System;
using UnityEngine;

/// <summary>Represents any of the 12 layers in an Environment</summary>
/// <remarks>
/// Handles and applies the parallax effect
/// </remarks>
public class BackgroundLayer : MonoBehaviour
{
    [field: SerializeField]
    public float ParallaxFactor { get; private set; }

    private Transform trans;
    private float startX;

    public GameObject Initialize()
    {
        trans = transform;
        this.startX = trans.position.x;
        return Instantiate(gameObject);
    }

    public void ApplyEnvironment(BackgroundLayer layer)
    {
        // TODO
    }

    private void Update()
    {
        this.trans.Translate(Vector3.left * (ParallaxFactor * Time.deltaTime));
        
        if (!(this.trans.position.x <= -this.startX)) return;

        Vector3 position = this.trans.position;
        position = new Vector3(this.startX, position.y, position.z);
        this.trans.position = position;
        
        // TODO: Notify the BackgroundManager that this layer looped
    }
}