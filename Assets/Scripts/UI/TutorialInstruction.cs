using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public sealed class TutorialInstruction : MonoBehaviour
{
    private void OnEnable()
    {
        // Check if is first time playing
        var prefs = SaveSystem.LoadPreferences();
        bool isFirstTime = prefs.IsFirstTime;

        if (!isFirstTime)
        {
            Destroy(gameObject);
            return;
        }
        
        EventManager.Events.OnPlayStart += DestroyOnPlay;
        prefs.IsFirstTime = false;
        SaveSystem.SavePreferences(prefs);
    }

    private void OnDisable()
    {
        EventManager.Events.OnPlayStart -= DestroyOnPlay;
    }

    private void DestroyOnPlay()
    {
        Destroy(gameObject);
    }
}
