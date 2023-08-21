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

        _text = GetComponent<Text>();
        
        EventManager.Events.OnTutorialSkulkAction += HandleSkulkUpdate;
        EventManager.Events.OnCompleteTutorialSkulks += DestroyOnFinish;

        this.enabled = false;
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
            this.enabled = true;
            _text.text = _tutorialTexts[_textId];
            _textId++;
            return;
        } 
        
        this.enabled = false;
        
    }

    private void DestroyOnFinish()
    {
        Destroy(gameObject);
    }
}
