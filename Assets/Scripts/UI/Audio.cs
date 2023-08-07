using UnityEngine;

public class Audio : MonoBehaviour
{
    private void Awake()
    {
        float volume = SaveSystem.LoadPreferences().Volume;
        GetComponent<AudioSource>().volume = volume;
    }
}
