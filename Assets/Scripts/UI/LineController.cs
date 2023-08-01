using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public sealed class LineController : MonoBehaviour
{
    private LineRenderer _lr;
    private GameObject _player;

    /// How thin the line should be
    [SerializeField] private float _lineWidth = 0.2f;

    // How long the line is
    [SerializeField] private float _lineLength = 0.3f;
    
    // Gap between Player sprite and speech line start
    [SerializeField] private float _playerLineOffset = 2f;

    /// Minimum angle the line should make with the horizontal
    private const float MinAngle = Mathf.PI / 6; // 30 deg
    
    /// Maximum angle the line should make with the horizontal
    private const float MaxAngle = Mathf.PI / 3; // 60 deg

    /// Left-side divisor, marking where the line should switch directions
    private const float NegativeWedge = -1.25f;
    
    /// Right-side divisor, marking where the line should switch directions
    private const float PositiveWedge = 1.25f;

    /// Makes using/visualizing numbers in the Editor a little easier
    private const float ParameterScaleFactor = 0.1f;

    /// Speech text for reference, used to set its position
    private SpeechText _speechText;

    private void OnEnable()
    {
        EventManager.Events.OnPauseKeyDown += SetLine;
    }

    private void OnDestroy()
    {
        EventManager.Events.OnPauseKeyDown -= SetLine;
    }

    private void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        SetWidth(_lineWidth * ParameterScaleFactor);
        _player = GameObject.FindWithTag("Player");
        _speechText = GameObject.FindWithTag("SpeechText")
            .GetComponent<SpeechText>();
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

    private void SetLine(bool isPaused)
    {
        gameObject.SetActive(isPaused);

        // Continue with vector calculations when game is paused
        if (!isPaused) return;

        Vector3 playerPosition = _player.transform.position;
        Vector3 direction = ChooseDirection(playerPosition.x);

        // Calculate start position with an gap/offset from the player
        Vector3 startPosition = playerPosition + direction *
            (_playerLineOffset * ParameterScaleFactor);

        // Calculate the end position based on the line length and player's position
        Vector3 endPosition = startPosition + direction * (_lineLength *
            ParameterScaleFactor);

        // Set the positions of the line renderer
        _lr.SetPosition(0, startPosition);
        _lr.SetPosition(1, endPosition);
        
        // Align text end position to start
        _speechText.SetPosition(endPosition);
        
        // Get full Text object width
        Vector2 textDim = _speechText.GetDimensions();
        float textWidth = textDim.x;
        
        // Create offset of half width (with customs)
        Vector2 offset = new Vector2((textWidth / 2) + SpeechText.XOffset,
            SpeechText.YOffset);
        if (direction.x < 0) offset.x *= -1;
        _speechText.SetOffset(offset);
        
        // Apply nudging if offscreen if necessary
        _speechText.NudgeAsNeeded();
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
