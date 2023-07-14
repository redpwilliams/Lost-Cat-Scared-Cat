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
  [SerializeField] private float stepsPadding;
  [SerializeField] private float stepsMultiplier;

  [Header("Pause Menu Props")]
  [SerializeField] private GameObject pauseMenu;
  
  [Range(0.01f, 1f)]
  [SerializeField] private float pauseMenuWidthFactor = 1f;
  
  [Range(0.01f, 1f)]
  [SerializeField] private float pauseMenuHeightFactor = 1f;

  /// BackgroundManager Instance,
  /// Used for scroll velocity
  private BackgroundManager bgm;

  /// UIManager RectTransform component
  private RectTransform rt;

  private void Awake()
  {
    if (bgm != null)
    {
      Destroy(ui);
      return;
    }
    
    ui = this;
    bgm = BackgroundManager.bgm;
    rt = GetComponent<RectTransform>();
  }

  private void Start()
  {
    InitMileage();
    InitPauseMenu();
  }

  private void Update()
  {
    mileage += bgm.GetScrollVelocity() * Time.deltaTime * stepsMultiplier;
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
    stepsPadding = 50f;
    stepsMultiplier = 3.0f;
    mileage = 0f;
    mileageText = mileageText.GetComponent<Text>();
    RectTransform mileageRT = mileageText.GetComponent<RectTransform>();
    mileageRT.anchoredPosition = new Vector2(-stepsPadding, mileageRT.anchoredPosition.y - mileageRT.rect.height);
  }

  /// Sets the Pause Menu size and location.
  /// Inits to covering the full screen 
  private void InitPauseMenu()
  {
    Rect rect = this.rt.rect;
    // Get Width and Height . . . 
    float width = rect.width * pauseMenuWidthFactor;
    float height = rect.height * pauseMenuHeightFactor;

    // . . . And Set
    this.rt.sizeDelta = new Vector2(width, height);

    // Disable PauseMenu by default
    pauseMenu.SetActive(false);
  }

  
  /// Defines what the PauseKey is
  private static bool PauseKeyDown() { return Input.GetKeyDown(KeyCode.Escape); }

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
}
