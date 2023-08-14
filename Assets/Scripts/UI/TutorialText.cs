using UnityEngine;

public sealed class TutorialText : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.Events.OnPlayStart += DestroyOnPlay;
    }

    private void OnDestroy()
    {
        EventManager.Events.OnPlayStart -= DestroyOnPlay;
    }

    private void DestroyOnPlay()
    {
        Destroy(gameObject);
    }
}
