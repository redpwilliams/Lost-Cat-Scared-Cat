using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Handles necessary button input throughout game</summary>
/// <remarks>
/// Not all buttons needed to be handled through this script.
/// For example, buttons that call `GameObject.SetActive` can be handled
/// natively in the editor.
/// </remarks>
[UsedImplicitly]
public sealed class ButtonManager : MonoBehaviour
{
    /// Handles the Title Screen's Play button by starting the Game scene
    [UsedImplicitly]
    public void HandlePlay()
    {
        SceneManager.LoadScene("Scenes/Game");
    }

    /// Handles the Title Screen's Quit button by exiting the application
    [UsedImplicitly]
    public void HandleQuit()
    {
        Application.Quit();
    }

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