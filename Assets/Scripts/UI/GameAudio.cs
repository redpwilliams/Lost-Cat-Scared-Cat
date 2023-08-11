using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public sealed class GameAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _forestMusic;
    [SerializeField] private AudioSource _mainMusic;

    [Tooltip("The ratio between the Background music and Main music volume levels")]
    [SerializeField] private float _volumeRatio = 3 / 5f;
    private void Awake()
    {
        float volume = SaveSystem.LoadPreferences().Volume;

        _mainMusic.volume = volume;
        _forestMusic.volume = volume * _volumeRatio;
    }

    private void OnEnable()
    {
        EventManager.Events.OnPlayStart += LoadMusic;
    }

    private void OnDisable()
    {
        EventManager.Events.OnPlayStart -= LoadMusic;
    }

    private void LoadMusic()
    {
        _mainMusic.Play();
    }
}
