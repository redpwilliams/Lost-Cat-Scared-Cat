using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>Handles necessary button input throughout game</summary>
/// <remarks>
/// Not all buttons needed to be handled through this script.
/// For example, buttons that call `GameObject.SetActive` can be handled
/// natively in the editor.
/// </remarks>
[UsedImplicitly]
public sealed class ButtonManager : MonoBehaviour
{
    private float _volumeLevel;
    
    /// Handles the Title Screen's Play button by starting the Game scene
    [UsedImplicitly]
    public void HandlePlay()
    {
        SceneManager.LoadScene("Scenes/Gameplay");
    }

    /// Handles the Title Screen's Quit button by exiting the application
    [UsedImplicitly]
    public void HandleQuit()
    {
        Application.Quit();
    }

    /// Handles moving the Volume slider
    [UsedImplicitly]
    public void HandleVolume(Slider slider)
    {
        AudioSource[] sources = FindObjectsOfType<AudioSource>();
        _volumeLevel = slider.value;
        foreach (var source in sources)
        {
            source.volume = _volumeLevel;
        }
    }

    /// Handles Returning from the Options screen
    [UsedImplicitly]
    public void HandleReturn()
    {
        // Save volume to player prefs
        var preferences = SaveSystem.LoadPreferences();
        preferences.Volume = _volumeLevel;
        SaveSystem.SavePreferences(preferences);
    }

    /// Handles pausing the game from the UI pause button
    [UsedImplicitly]
    public void HandlePause()
    {
        EventManager.Events.PauseKeyDown();
    }

    /// Handles resuming from the Pause Menu
    [UsedImplicitly]
    public void HandleResume()
    {
        EventManager.Events.PauseKeyDown();
    }

    /// Handles the Pause Menu's Quit button by returning to the title screen
    [UsedImplicitly]
    public void HandleReturnToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scenes/Title Screen");
    }
}