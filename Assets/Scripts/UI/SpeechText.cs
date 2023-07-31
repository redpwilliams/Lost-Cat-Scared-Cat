using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SpeechText : MonoBehaviour
{
    [SerializeField] private string[] phrases;
    private Text _text;
    private RectTransform _rt;

    private void OnEnable()
    {
        EventManager.Events.OnPauseKeyDown += SetText;
    }
    
    private void OnDestroy()
    {
        EventManager.Events.OnPauseKeyDown -= SetText;
    }

    private void OnValidate()
    {
        if (phrases.Length > 0) return;
        Debug.LogError("Array `phrases` must at least be of length 1");
    }

    private void Awake()
    {
        _text = GetComponent<Text>();
        _rt = transform as RectTransform;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void SetText(bool isPaused)
    {
        gameObject.SetActive(isPaused);
        
        // Continue to set text if game is paused
        if (!isPaused) return;

        // Set phrase
        _text.text = phrases[Random.Range(0, phrases.Length - 1)];
        
        // Set Text box width to fit phrase
        float textWidth = _text.preferredWidth;
        
        // Update the width of the RectTransform to match
        Vector2 sizeDelta = _rt.sizeDelta;
        sizeDelta.x = textWidth;
        _rt.sizeDelta = sizeDelta;
    }

    public void SetPosition(Vector3 lineEndPosition)
    {
        transform.position = lineEndPosition;
    }

    public void SetOffset(float offset)
    {
        Vector2 currentPos = _rt.anchoredPosition;
        _rt.anchoredPosition = new Vector2(currentPos.x + offset,
            currentPos.y);
    }

    public float GetWidth()
    {
        return _text.preferredWidth;
    }
}
