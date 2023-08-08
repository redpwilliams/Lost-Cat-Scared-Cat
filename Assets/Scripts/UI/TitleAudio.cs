using UnityEngine;

public class TitleAudio : MonoBehaviour
{
    private void Awake()
    {
        float volume = SaveSystem.LoadPreferences().Volume;
        GetComponent<AudioSource>().volume = volume;
    }
}
