using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
  private float mileage;
  [SerializeField] private Text mileageText;
  [SerializeField] private float padding;
  [SerializeField] private float stepsMultiplier;

  // Used for scroll velocity
  [SerializeField] private BackgroundManager bgm;

  void Start()
  {
    padding = 50f;
    stepsMultiplier = 3.0f;
    mileage = 0f;
    mileageText = mileageText.GetComponent<Text>();
    RectTransform mrt = mileageText.GetComponent<RectTransform>();
    mrt.anchoredPosition = new Vector2(-padding, mrt.anchoredPosition.y - mrt.rect.height);

    bgm = bgm.GetComponent<BackgroundManager>();
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
}
