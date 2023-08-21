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
        _text.text = _tutorialTexts[_textId];
        _textId++;
    }

    private void OnDisable()
    {
    }

    private void HandleSkulkUpdate(bool startedSpawn)
    {
        if (startedSpawn)
        {
            this.enabled = true;
            return;
        } 
        
        this.enabled = false;
        
    }

    private void DestroyOnFinish()
    {
        Destroy(gameObject);
    }
}
