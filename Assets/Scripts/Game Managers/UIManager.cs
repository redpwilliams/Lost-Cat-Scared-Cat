using UnityEngine;
using UnityEngine.UI;

public sealed class UIManager : MonoBehaviour
{
    /// Singleton Instance
    public static UIManager ui;

    /// Pause state
    private bool gameIsPaused;

    /// Mileage / Number of steps / Distance Player has traveled
    private float mileage;

    [Header("Mileage Props")] 
    [SerializeField] private Text mileageText;

    [SerializeField] private float stepsMultiplier = 3f;

    /// Pause Menu GameObject
    [SerializeField] private GameObject pauseMenu;

    /// Heart GameObject
    [SerializeField] private GameObject heart;

    private GameObject[] hearts;
    private int numHeartsShown;

    /// SpriteRenderer of the active/top-most heart
    private SpriteRenderer sr;

    private void Awake()
    {
        if (ui != null)
        {
            Destroy(ui);
            return;
        }

        ui = this;
        hearts = new GameObject[Player.NumLives];
    }

    private void Start()
    {
        InitMileage();
        InitPauseMenu();
        InitHearts(Player.NumLives);
    }

    private void Update()
    {
        mileage += BackgroundManager.bgm.GetScrollVelocity() * Time.deltaTime * stepsMultiplier;
        SetMileageText();

        // Handle Pause
        if (PauseKeyDown()) HandlePause();
    }

    /// Updates the mileage field on the screen
    private void SetMileageText()
    {
        // Update mileage
        mileageText.text = $"{this.mileage:#} steps";
    }

    /// Sets the Mileage text on the screen.
    /// Customizable in GameObject Component
    private void InitMileage()
    {
        mileage = 0;
        mileageText = mileageText.GetComponent<Text>();
    }

    /// Sets the Pause Menu size and location.
    /// Inits to covering the full screen 
    private void InitPauseMenu()
    {
        // Disable PauseMenu by default
        pauseMenu.SetActive(false);
    }

    /// Sets the number of hearts based on the Player class
    private void InitHearts(int numHearts)
    {
        hearts[0] = this.heart;
        Vector3 heartPos = this.heart.GetComponent<RectTransform>().position;

        for (int i = 1; i < numHearts; i++)
        {
            // Create new game object
            GameObject heartClone = Instantiate(heart, transform);
            heartClone.name = $"Heart ({i + 1})";

            // Set its position
            Vector3 clonePos = new Vector3(heartPos.x - 0.075f * i, heartPos.y,
                heartPos.z);
            heartClone.GetComponent<RectTransform>().position = clonePos;

            // Add it to the list
            hearts[i] = heartClone;
        }

        this.numHeartsShown = numHearts;
    }

    /// Defines what the PauseKey is
    private static bool PauseKeyDown()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }

    /// Handles what pressing Pause does
    public void HandlePause()
    {
        if (this.gameIsPaused)
        {
            // Resume
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            this.gameIsPaused = false;
            return;
        }

        // Pause
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        this.gameIsPaused = true;
    }

    public void LoseHeart()
    {
        if (this.numHeartsShown == 0) return;

        // Get current heart game object and destroy
        Heart currentHeart =
            this.hearts[this.numHeartsShown - 1].GetComponent<Heart>();
        currentHeart.Destroy();

        // Update list
        this.numHeartsShown--;
        this.hearts[this.numHeartsShown] = null;

        // TODO - Handle GameOver
        if (this.numHeartsShown == 0) Debug.Log("Lost last heart, game over");
    }
}