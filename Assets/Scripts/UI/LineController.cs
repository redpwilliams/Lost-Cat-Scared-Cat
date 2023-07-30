using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineController : MonoBehaviour
{
    private LineRenderer _lr;
    private GameObject _player;

    /// How thin the line should be
    [SerializeField] private float lineWidth = 0.02f;

    /// Minimum angle the line should make with the horizontal
    private const float MinAngle = Mathf.PI / 6; // 30 deg
    
    /// Maximum angle the line should make with the horizontal
    private const float MaxAngle = Mathf.PI / 3; // 60 deg

    /// Left-side divisor, marking where the line should switch directions
    private const float NegativeWedge = -1f;
    
    /// Right-side divisor, marking where the line should switch directions
    private const float PositiveWedge = 1f;

    private void OnEnable()
    {
        EventManager.Events.OnPauseKeyDown += (b =>
        {
            gameObject.SetActive(b);
            if (!b) return;
            float lineLength = 0.3f;
            Vector3 playerPosition = _player.transform.position;

            Vector3 direction = ChooseEndpoint(playerPosition.x);

            // Calculate the end position based on the line length and player's position
            Vector3 endPosition = playerPosition + direction * lineLength;

            // Set the positions of the line renderer
            _lr.SetPosition(0, playerPosition);
            _lr.SetPosition(1, endPosition);
        });
    }

    private void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        SetWidth(lineWidth);
        _player = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void SetWidth(float width)
    {
        _lr.startWidth = width;
        _lr.endWidth = width;
    }

    private static Vector3 ChooseEndpoint(float playerXPos)
    {
        float theta = Random.Range(MinAngle, MaxAngle);
        float dir = playerXPos is < NegativeWedge or < PositiveWedge and >= 0 ? 1f : -1f;

        float x = Mathf.Cos(theta) * dir;
        float y = Mathf.Sin(theta);
        return new Vector3(x, y, 0);
    }
}
