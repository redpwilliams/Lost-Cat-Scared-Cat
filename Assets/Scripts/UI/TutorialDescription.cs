using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public sealed class TutorialDescription : MonoBehaviour
{
    private Text _text;
    private int _textId;
    private readonly string[] _tutorialTexts = 
    {
        "Red foxes run straight through",
        "Brown foxes jump when they are ready",
        "Lastly, Gray foxes sprint when the time is right"
    };
    
    private void OnEnable()
    {
        _text = GetComponent<Text>();
        _text.text = _tutorialTexts[_textId];
        _textId++;
    }
}
