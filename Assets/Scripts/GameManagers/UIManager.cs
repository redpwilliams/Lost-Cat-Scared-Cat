using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
  // Singleton Instance
  public static UIManager ui;
  // Pause state
  public static bool GameIsPaused = false;

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

  // Used for scroll velocity
  [Header("Background Manager Instance")]
  [SerializeField] private BackgroundManager bgm;

  // UIManager RectTransform componenet
  private RectTransform rt;

  private void Awake()
  {
    if (ui == null) ui = this;
    else Destroy(ui);
  }

  void Start()
  {
    bgm = bgm.GetComponent<BackgroundManager>();
    rt = GetComponent<RectTransform>();

    InitMileage();
    InitPauseMenu();
  }

  void Update()
  {
    mileage += bgm.GetScrollVelocity() * Time.deltaTime * stepsMultiplier;
    SetMileageText();

    // Handle Pause
    if (PauseKeyDown()) HandlePause();
  }

  /// <summary>
  /// Updates the mileage field on the screen
  /// </summary>
  void SetMileageText()
  {
    // Update mileage
    mileageText.text = String.Format("{0:#} steps", mileage);
  }

  /// <summary>
  /// Sets the Mileage text on the screen
  /// Customizable in GameObject Component
  /// </summary>
  void InitMileage()
  {
    stepsPadding = 50f;
    stepsMultiplier = 3.0f;
    mileage = 0f;
    mileageText = mileageText.GetComponent<Text>();
    RectTransform mileageRT = mileageText.GetComponent<RectTransform>();
    mileageRT.anchoredPosition = new Vector2(-stepsPadding, mileageRT.anchoredPosition.y - mileageRT.rect.height);
  }

  /// <summary>
  /// Sets the Pause Menu size and location
  /// Inits to covering the full screen 
  /// </summary>
  void InitPauseMenu()
  {
    RectTransform pauseMenuRT = pauseMenu.GetComponent<RectTransform>();

    // Get Width and Height . . . 
    float width = rt.rect.width * pauseMenuWidthFactor;
    float height = rt.rect.height * pauseMenuHeightFactor;

    // . . . And Set
    pauseMenuRT.sizeDelta = new Vector2(width, height);

    // Disable PauseMenu by default
    pauseMenu.SetActive(false);
  }

  
  /// <summary>
  /// Defines what the PauseKey is
  /// </summary>
  /// <returns></returns>
  bool PauseKeyDown() { return Input.GetKeyDown(KeyCode.Escape); }

  /// <summary>
  /// Handles what pressing Pause does
  /// </summary>
  public void HandlePause()
  {
    if (GameIsPaused)
    {
      // Resume
      pauseMenu.SetActive(false);
      Time.timeScale = 1f;
      GameIsPaused = false;
      return;
    }

    // Pause
    pauseMenu.SetActive(true);
    Time.timeScale = 0f;
    GameIsPaused = true;
  }
}
