using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Text))]
public sealed class SpeechText : MonoBehaviour
{
    [SerializeField] private string[] _phrases;
    private string[] _complement;
    private int _phraseIndex;
    
    private Text _text;
    private RectTransform _rt;

    public const float XOffset = 30f;
    public const float YOffset = 50f;

    private const float SidePadding = 0.1f;

    private Camera _cam;

    private void OnDestroy()
    {
        EventManager.Events.OnPauseKeyDown -= SetText;
    }

    private void OnValidate()
    {
        if (_phrases.Length > 0) return;
        Debug.LogError("Array `phrases` must at least be of length 1");
    }

    private void Awake()
    {
        EventManager.Events.OnPauseKeyDown += SetText;
        
        _text = GetComponent<Text>();
        _rt = transform as RectTransform;
        _cam = Camera.main;
        _complement = new string[_phrases.Length - 1];
        // Copy all but last element of phrases array
        for (int i = 0; i < _complement.Length; i++)
        {
            _complement[i] = _phrases[i];
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void SetText(bool isPaused)
    {
        gameObject.SetActive(isPaused);
        // Debug.Log("Called SetText");
        
        // Continue to set text if game is paused
        if (!isPaused) return;

        // Set phrase
        _text.text = GetNextPhrase();
        
        // Set Text box dimensions to fit phrase
        float textWidth = _text.preferredWidth;
        float textHeight = _text.preferredHeight;
        
        // Update the dimensions of the RectTransform to match
        Vector2 sizeDelta = _rt.sizeDelta;
        sizeDelta.x = textWidth;
        sizeDelta.y = textHeight;
        _rt.sizeDelta = sizeDelta;
    }

    public void SetPosition(Vector3 lineEndPosition)
    {
        transform.position = lineEndPosition;
    }

    public void SetOffset(Vector2 offset)
    {
        Vector2 currentPos = _rt.anchoredPosition;
        _rt.anchoredPosition = new Vector2(currentPos.x + offset.x,
            currentPos.y + offset.y);
    }

    public Vector2 GetDimensions()
    {
        return new Vector2(_text.preferredWidth, _text.preferredHeight);
    }

    public void NudgeAsNeeded()
    {
        // Check if the Text is off-screen
        Vector3[] corners = new Vector3[4];
        _rt.GetWorldCorners(corners);

        // Get the screen boundaries in world space
        Vector3 minScreenBounds = _cam.ScreenToWorldPoint(Vector3.zero);
        Vector3 maxScreenBounds = _cam.ScreenToWorldPoint(new Vector3(
            Screen.width,
            Screen.height,
            0f));
        
        Transform trans = transform;

        // Check if the text is off-screen on the right side
        if (corners[2].x > maxScreenBounds.x)
        {
            // Calculate the horizontal distance the text is off-screen
            float distanceOffScreen = corners[2].x - maxScreenBounds.x;

            // Nudge the text left by the distance it's off-screen + padding
            Vector3 newPosition = trans.position;
            newPosition.x -= distanceOffScreen + SidePadding;
            trans.position = newPosition;
        }
        // Check if the text is off-screen on the left side
        else if (corners[0].x < minScreenBounds.x)
        {
            // Calculate the horizontal distance the text is off-screen
            float distanceOffScreen = minScreenBounds.x - corners[0].x;

            // Nudge the text right by the distance it's off-screen
            Vector3 newPosition = trans.position;
            newPosition.x += distanceOffScreen + SidePadding;
            transform.position = newPosition;
        }
    }

    private void FillComplementaryArray()
    {
        for (int i = 0, j = 0; i < _complement.Length; i++, j++)
        {
            if (j == _phraseIndex) j++;
            _complement[i] = _phrases[j];
        }
    }

    private string GetNextPhrase()
    {
        // If there is only one phrase, return that phrase
        if (_phrases.Length == 1) return _phrases[0];
        
        FillComplementaryArray();
        
        string nextPhrase = _complement[Random.Range(0, _complement.Length)];
        Debug.Log(nextPhrase);
        _phraseIndex = Array.IndexOf(_phrases, nextPhrase);
        return nextPhrase;
    }
}
