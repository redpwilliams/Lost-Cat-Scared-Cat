using System;
using UnityEngine;

/// <summary>Handles all events in the game</summary>
/// <remarks>
/// EventManager provides implementation for the subscription and dispatching of
/// all events in the game. <para />
/// To subscribe to an event,
/// <code>EventManager.events.EventAction += YouMethod</code><para />
/// To dispatch an event, <code>EventManager.events.OnYourEvent()</code>
/// </remarks>
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
    public event Action OnFoxHitsPlayer;

    /// Handles when a Fox collides with the player
    public void FoxHitsPlayer()
    {
        OnFoxHitsPlayer?.Invoke();
    }

    /// Player is invincible event
    public event Action OnPlayerInvincible;

    public void PlayerInvincible()
    {
        OnPlayerInvincible?.Invoke();
    }

    /// Player is vulnerable event
    public event Action OnPlayerVulnerable;

    public void PlayerVulnerable()
    {
        OnPlayerVulnerable?.Invoke();
    }

}
