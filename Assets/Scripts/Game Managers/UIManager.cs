using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public sealed class UIManager : MonoBehaviour
{
    /// Singleton Instance
    public static UIManager ui;

    /// Mileage / Number of steps / Distance Player has traveled
    private float _mileage;

    [Header("Mileage Props")] 
    [SerializeField] private Text _mileageText;
    private const float StepsMultiplier = 3f;

    /// True if game is paused
    private bool _isPaused;

    /// Heart GameObject
    [SerializeField] private GameObject _heart;

    private GameObject[] _hearts;
    private int _numHeartsShown;

    /// SpriteRenderer of the active/top-most heart
    private SpriteRenderer _sr;

    private void OnEnable()
    {
        EventManager.Events.OnPauseKeyDown += HandlePause;
    }

    private void OnDisable()
    {
        EventManager.Events.OnPauseKeyDown -= HandlePause;
    }

    private void Awake()
    {
        if (ui != null)
        {
            Destroy(ui);
            return;
        }

        ui = this;
        _hearts = new GameObject[Player.NumLives];
    }

    private void Start()
    {
        InitMileage();
        InitHearts(Player.NumLives);
    }

    private void Update()
    {
        _mileage += BackgroundManager.bgm.ScrollVelocity * Time.deltaTime * 
        StepsMultiplier;
        SetMileageText();

        // Handle Pause
        if (!HasInputPause()) return;
        
        _isPaused ^= true;
        EventManager.Events.PauseKeyDown(_isPaused);
    }

    public void LoseHeart()
    {
        if (_numHeartsShown == 0) return;

        // Get current heart game object and destroy
        Heart currentHeart =
            _hearts[this._numHeartsShown - 1].GetComponent<Heart>();
        currentHeart.Destroy();

        // Update list
        _numHeartsShown--;
        _hearts[this._numHeartsShown] = null;

        // TODO - Handle GameOver
        if (_numHeartsShown == 0) Debug.Log("Lost last heart, game over");
    }

    /// Updates the mileage field on the screen
    private void SetMileageText()
    {
        // Update mileage
        _mileageText.text = $"{_mileage:#} steps";
    }

    /// Sets the Mileage text on the screen.
    /// Customizable in GameObject Component
    private void InitMileage()
    {
        _mileage = 0;
    }

    /// Sets the number of hearts based on the Player class
    private void InitHearts(int numHearts)
    {
        _hearts[0] = _heart;
        Vector3 heartPos = _heart.GetComponent<RectTransform>().position;

        for (int i = 1; i < numHearts; i++)
        {
            // Create new game object
            GameObject heartClone = Instantiate(_heart, transform);
            heartClone.name = $"Heart ({i + 1})";

            // Set its position
            Vector3 clonePos = new Vector3(heartPos.x - 0.075f * i, heartPos.y,
                heartPos.z);
            heartClone.GetComponent<RectTransform>().position = clonePos;

            // Add it to the list
            _hearts[i] = heartClone;
        }

        _numHeartsShown = numHearts;
    }

    /// Defines what the PauseKey is
    private static bool HasInputPause()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }

    private void HandlePause(bool isNowPaused)
    {
        _isPaused = isNowPaused;
    }
}