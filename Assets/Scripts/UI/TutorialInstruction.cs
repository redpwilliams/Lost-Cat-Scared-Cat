using UnityEngine;

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
    }

    private void OnDisable()
    {
        /*
         * REVIEW: This is where I decide the player
         * shouldn't see the tutorial text again.
         * If they decide to quit the game before the other
         * tutorial text occurs, then that's on them
         *
         * This way, too, TutorialDescription is not
         * also setting IsFirstTime.
         */
        var prefs = SaveSystem.LoadPreferences();
        prefs.IsFirstTime = false;
        SaveSystem.SavePreferences(prefs);
        
        EventManager.Events.OnPlayStart -= DestroyOnPlay;
    }

    private void DestroyOnPlay()
    {
        Destroy(gameObject);
    }
}
