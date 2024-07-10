using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public sealed class PauseButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        EventManager.Events.OnPauseKeyDown += OnPressed;
    }

    private void OnDestroy()
    {
        
        EventManager.Events.OnPauseKeyDown -= OnPressed;
    }

    private void OnPressed(bool isPaused)
    {
        _button.enabled = !isPaused;
    }
    
}
