using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public sealed class GameAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _forestMusic;
    [SerializeField] private AudioSource _mainMusic;

    [Tooltip("The ratio between the Background music and Main music volume levels")]
    [SerializeField] private float _volumeRatio = 3 / 5f;

    [SerializeField] private float _fadeDuration = 1.5f;
    
    private void Awake()
    {
        float volume = SaveSystem.LoadPreferences().Volume;

        _mainMusic.volume = volume;
        _forestMusic.volume = volume * _volumeRatio;
    }

    private void OnEnable()
    {
        EventManager.Events.OnPlayStart += LoadMusic;
        EventManager.Events.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        EventManager.Events.OnPlayStart -= LoadMusic;
        EventManager.Events.OnGameOver -= HandleGameOver;
    }

    private void LoadMusic()
    {
        _mainMusic.Play();
    }

    private void HandleGameOver()
    {
        StartCoroutine(FadeOutGameMusic());
    }

    private IEnumerator FadeOutGameMusic()
    {
        // Should be less than the hang time between scenes
        // I don't feel like getting that dynamically

        float originalMainVolume = _mainMusic.volume;
        float originalForestVolume = _forestMusic.volume;
        
        float elapsedTime = 0f;
        while (elapsedTime < _fadeDuration)
        {
            float t = elapsedTime / _fadeDuration;
            
            _mainMusic.volume = Mathf.Lerp(originalMainVolume, 0, t);
            _forestMusic.volume = Mathf.Lerp(originalForestVolume, 0, t);
            
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        _mainMusic.volume = 0;
    }
}
