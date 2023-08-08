using UnityEngine;

public class Title : MonoBehaviour
{
    private void Awake()
    {
        float volume = SaveSystem.LoadPreferences().Volume;
        GetComponent<AudioSource>().volume = volume;
    }
}
