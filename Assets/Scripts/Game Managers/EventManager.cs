using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    /// Singleton Instance
    public static EventManager events;

    private void Awake()
    {
        if (events != null)
        {
            Destroy(events);
            return;
        }

        events = this;
    }

    /// Fox hits player event
    public event Action FoxHitsPlayer;

    /// Handles when a Fox collides with the player
    public void OnFoxHitsPlayer()
    {
        FoxHitsPlayer?.Invoke();
    }

}
