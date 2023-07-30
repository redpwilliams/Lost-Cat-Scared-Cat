using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private const float PauseTimeScale = 0f;

    private void Awake()
    {
        EventManager.Events.OnPauseKeyDown += HandlePause;
    }

    private void OnDestroy()
    {
        EventManager.Events.OnPauseKeyDown -= HandlePause;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    /// Handles what pressing Pause does
    public void HandlePause(bool isNowPaused)
    {
        if (isNowPaused)
        {
            // Pause
            gameObject.SetActive(true);
            Time.timeScale = PauseTimeScale;
        }
        else
        {
            // Resume
            gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
