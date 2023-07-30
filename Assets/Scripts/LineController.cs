using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    private GameObject player;

    /// How thin the line should be
    [SerializeField] private float lineWidth = 0.02f;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        SetWidth(this.lineWidth);
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        this.lr.SetPosition(0, this.player.transform.position);
    }

    private void SetWidth(float width)
    {
        this.lr.startWidth = width;
        this.lr.endWidth = width;
    }
}
