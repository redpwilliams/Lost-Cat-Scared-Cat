using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public sealed class Score : MonoBehaviour
{
    private Text _scoreText;
    
    private void Start()
    {
        _scoreText = GetComponent<Text>();
        _scoreText.text = $"Score: {EventManager.Events.FinalScore}";
        
        // Handle high score, or in EventManager as an extra param to GameOver
    }
}
