using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
  private float mileage;

  [Header("Mileage Props")]
  [SerializeField] private Text mileageText;
  [SerializeField] private float stepsPadding;
  [SerializeField] private float stepsMultiplier;

  [Header("Start Screen Props")]
  [SerializeField] private GameObject pauseMenu;
  [SerializeField] private Vector2 origin;


  // Used for scroll velocity
  [Header("BGM Instance")]
  [SerializeField] private BackgroundManager bgm;

  // UIManager RectTransform componenet
  private RectTransform rt;

  void Start()
  {
    bgm = bgm.GetComponent<BackgroundManager>();
    rt = GetComponent<RectTransform>();

    InitMileage();
    InitStartScreen();
  }

  void Update()
  {
    mileage += bgm.GetScrollVelocity() * Time.deltaTime * stepsMultiplier;
    SetText();
  }

  void SetText()
  {
    // Update mileage
    mileageText.text = String.Format("{0:#} steps", mileage);
  }

  void InitMileage()
  {
    stepsPadding = 50f;
    stepsMultiplier = 3.0f;
    mileage = 0f;
    mileageText = mileageText.GetComponent<Text>();
    RectTransform mileageRT = mileageText.GetComponent<RectTransform>();
    mileageRT.anchoredPosition = new Vector2(-stepsPadding, mileageRT.anchoredPosition.y - mileageRT.rect.height);
  }

  void InitStartScreen()
  {
    RectTransform pauseMenuRT = pauseMenu.GetComponent<RectTransform>();

    // Width and Height
    float width = rt.rect.width;
    float height = rt.rect.height;

    // Set
    pauseMenuRT.sizeDelta = new Vector2(width, height);

    Debug.Log(pauseMenuRT.rect.width);
    Debug.Log(pauseMenuRT.rect.height);
  }
}
