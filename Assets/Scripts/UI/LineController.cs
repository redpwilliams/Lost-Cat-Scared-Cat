using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineController : MonoBehaviour
{
    private LineRenderer _lr;
    private GameObject _player;

    /// How thin the line should be
    [SerializeField] private float lineWidth = 0.2f;

    // How long the line is
    [SerializeField] private float lineLength = 0.3f;
    
    // Gap between Player sprite and speech line start
    [SerializeField] private float playerLineOffset = 2f;

    /// Minimum angle the line should make with the horizontal
    private const float MinAngle = Mathf.PI / 6; // 30 deg
    
    /// Maximum angle the line should make with the horizontal
    private const float MaxAngle = Mathf.PI / 3; // 60 deg

    /// Left-side divisor, marking where the line should switch directions
    private const float NegativeWedge = -1f;
    
    /// Right-side divisor, marking where the line should switch directions
    private const float PositiveWedge = 1f;

    /// Makes using/visualizing numbers in the Editor a little easier
    private const float ParameterScaleFactor = 0.1f;

    private void OnEnable()
    {
        EventManager.Events.OnPauseKeyDown += (b =>
        {
            gameObject.SetActive(b);
            
            // Continue with vector calculations when game is paused
            if (!b) return;
            
            Vector3 playerPosition = _player.transform.position;
            Vector3 direction = ChooseDirection(playerPosition.x);

            // Calculate start position with an gap/offset from the player
            Vector3 startPosition = playerPosition + direction *
                (playerLineOffset * ParameterScaleFactor);

            // Calculate the end position based on the line length and player's position
            Vector3 endPosition = startPosition + direction * (lineLength * 
                ParameterScaleFactor);

            // Set the positions of the line renderer
            _lr.SetPosition(0, startPosition);
            _lr.SetPosition(1, endPosition);
        });
    }

    private void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        SetWidth(lineWidth * ParameterScaleFactor);
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

    private static Vector3 ChooseDirection(float playerXPos)
    {
        float theta = Random.Range(MinAngle, MaxAngle);
        float dir = playerXPos is < NegativeWedge or < PositiveWedge and >= 0 ? 1f : -1f;

        float x = Mathf.Cos(theta) * dir;
        float y = Mathf.Sin(theta);
        return new Vector3(x, y, 0);
    }
}
