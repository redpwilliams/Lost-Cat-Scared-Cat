using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public sealed class GameAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _forestMusic;
    [SerializeField] private AudioSource _mainMusic;

    [Tooltip("The ratio between the Forest music and Main music volume levels")]
    [SerializeField] private float _volumeRatio = 5 / 3f;
    private void Awake()
    {
        float volume = SaveSystem.LoadPreferences().Volume;

        _forestMusic.volume = volume;
        _mainMusic.volume = volume * (1 / _volumeRatio);
    }
}
