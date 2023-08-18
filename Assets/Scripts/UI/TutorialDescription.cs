using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public sealed class TutorialDescription : MonoBehaviour
{
    private Text _text;
    private int _textId;
    private readonly string[] _tutorialTexts = 
    {
        "Red foxes run straight through . . .",
        "Brown foxes jump when they are ready . . .",
        "And, Gray foxes sprint once they are prepared."
    };
    
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

        _text = GetComponent<Text>();
        
        EventManager.Events.OnTutorialSkulkAction += HandleSkulkUpdate;
        EventManager.Events.OnCompleteTutorialSkulks += DestroyOnFinish;
        
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        EventManager.Events.OnTutorialSkulkAction -= HandleSkulkUpdate;
        EventManager.Events.OnCompleteTutorialSkulks -= DestroyOnFinish;
    }

    private void HandleSkulkUpdate(bool startedSpawn)
    {
        if (startedSpawn)
        {
            _text.text = _tutorialTexts[_textId];
            gameObject.SetActive(true);
            _textId++;
            return;
        } 
        
        gameObject.SetActive(false);
        
    }

    private void DestroyOnFinish()
    {
        Destroy(gameObject);
    }
}
