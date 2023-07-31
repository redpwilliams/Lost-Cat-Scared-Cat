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
public sealed class EventManager : MonoBehaviour
{
    /// Singleton Instance
    public static EventManager Events { get; private set; }

    private void Awake()
    {
        if (Events != null && Events != this)
        {
            Destroy(Events);
            return;
        }

        Events = this;
        DontDestroyOnLoad(gameObject);
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

    /// Active/Deactivate Pause Menu
    public event Action<bool> OnPauseKeyDown;
    
    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>Fires when a change in Pause status is detected</summary>
    /// <remarks>
    /// If `isPaused` is true, Player just paused the game,
    /// and logic of the paused state should be run.
    /// If  `isPaused` is false, Player just unpaused the game,
    /// and logic of the unpaused state should be run.
    /// </remarks>
    public void PauseKeyDown(bool isPaused)
    {
        OnPauseKeyDown?.Invoke(isPaused);
    }

    /// Options screen cat is selected
    public event Action<uint> OnCatSelect;

    private uint _currentCatID;

    public void CatSelect(uint index)
    {
        // Ignore if user selects the already active Cat selection
        if (index == this._currentCatID) return;
        OnCatSelect?.Invoke(index);
        this._currentCatID = index;
        SaveSystem.SavePreferences(new Preferences(false, index));
    }

}
