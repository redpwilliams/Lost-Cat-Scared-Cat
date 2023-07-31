using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class UIManager : MonoBehaviour
{
    /// Singleton Instance
    public static UIManager ui;

    /// Mileage / Number of steps / Distance Player has traveled
    private float mileage;

    [Header("Mileage Props")] 
    [SerializeField] private Text mileageText;
    private const float StepsMultiplier = 3f;

    /// True if game is paused
    private bool isPaused;

    /// Heart GameObject
    [SerializeField] private GameObject heart;

    private GameObject[] hearts;
    private int numHeartsShown;

    /// SpriteRenderer of the active/top-most heart
    private SpriteRenderer sr;

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
        hearts = new GameObject[Player.NumLives];
    }

    private void Start()
    {
        InitMileage();
        InitHearts(Player.NumLives);
    }

    private void Update()
    {
        mileage += BackgroundManager.bgm.GetScrollVelocity() * Time.deltaTime * StepsMultiplier;
        SetMileageText();

        // Handle Pause
        if (!HasInputPause()) return;
        
        this.isPaused ^= true;
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        EventManager.Events.PauseKeyDown(this.isPaused);
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
    private static bool HasInputPause()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }

    private void HandlePause(bool isNowPaused)
    {
        isPaused = isNowPaused;
    }
}