using UnityEngine;

public class Bounds : MonoBehaviour
{
    [SerializeField] private Bounds _oppositeBounds;
    private float _xPos;

    private Player _player;
    
    private void Start()
    {
        _xPos = _oppositeBounds.transform.position.x;
        _player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        Transform playerTransform = _player.transform;

        playerTransform.position = new Vector3(_xPos * 0.95f, playerTransform
            .position.y, 0);
    }
}
