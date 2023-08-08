using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public sealed class TitleAudio : MonoBehaviour
{
    private void Awake()
    {
        float volume = SaveSystem.LoadPreferences().Volume;
        GetComponent<AudioSource>().volume = volume;
    }
}
