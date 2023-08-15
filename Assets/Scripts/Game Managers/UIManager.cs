using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public sealed class UIManager : MonoBehaviour
{
    /// Singleton Instance
    public static UIManager ui;
    
    /// SpriteRenderer of the active/top-most heart
    private SpriteRenderer _sr;

    [Header("Mileage Props")] 
    [SerializeField] private Text _mileageText;
    [SerializeField] private Text _highScoreText;
    private const float StepsMultiplier = 5f;
    
    /// Mileage / Number of steps / Distance Player has traveled
    private float _mileage;

    /// Heart GameObject
    [SerializeField] private GameObject _heart;
    private GameObject[] _hearts;
    private int _numHeartsShown;

    /// Time to wait before switching to Game Over screen after death
    [SerializeField] private float _transitionHangTime = 2.5f;

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


    private void OnApplicationFocus(bool hasFocus)
    {
        // Pause when loss of focus
        if (!hasFocus) EventManager.Events.PauseKeyDown();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        // Specifically for Android, pauses when player presses home
        if (pauseStatus) EventManager.Events.PauseKeyDown();
    }

    private void Start()
    {
        InitMileage();
        InitHearts(Player.NumLives);

        int highScore = SaveSystem.LoadPreferences().HighScore;
        _highScoreText.text = $"Best: {highScore:n0}";
    }

    private void Update()
    {
        _mileage += BackgroundManager.bgm.ScrollVelocity * Time.deltaTime *
                    StepsMultiplier;
        SetMileageText();

        // Handle Pause through Escape
        if (!HasInputPause()) return;
        
        EventManager.Events.PauseKeyDown();
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

    /// Updates the mileage field on the screen
    private void SetMileageText()
    {
        // Update mileage
        _mileageText.text = $"{Mathf.RoundToInt(_mileage):n0} steps";
    }
    
    /// Subtracts a heart from the UI and returns the number of hearts left
    /// Reported by the player
    public int LoseHeart()
    {
        // Failsafe, ensures no extra life loss after Player dies
        if (_numHeartsShown == 0) return 0;

        // Get current heart game object and destroy
        Heart currentHeart =
            _hearts[_numHeartsShown - 1].GetComponent<Heart>();
        currentHeart.Destroy();

        // Update list
        _numHeartsShown--;
        _hearts[_numHeartsShown] = null;

        if (_numHeartsShown != 0) return _numHeartsShown;
        
        // Game Over
        EventManager.Events.GameOver(_mileage);
        HandleGameOver();
        return 0;
    }

    /// Defines what the PauseKey is
    private static bool HasInputPause()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }

    private void HandleGameOver()
    {
        StartCoroutine(ToGameOverScreen());
    }

    private IEnumerator ToGameOverScreen()
    {
        yield return new WaitForSeconds(_transitionHangTime);
        SceneManager.LoadScene("Game Over");
    }
    
}