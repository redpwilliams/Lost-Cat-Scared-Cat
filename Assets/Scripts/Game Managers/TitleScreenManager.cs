using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class TitleScreenManager : MonoBehaviour
{
    [UsedImplicitly]
    public void HandlePlay()
    {
        SceneManager.LoadScene("Scenes/Game");
    }

    [UsedImplicitly]
    public void HandleQuit()
    {
        Application.Quit();
    }
}