using System;
using UnityEngine;

/// <summary>Handles all events in the game</summary>
/// <remarks>
///     EventManager provides implementation for the subscription and dispatching of
///     all events in the game. <para />
///     To subscribe to an event,
///     <code>
///         EventManager.events.EventAction += MyMethod
///     </code>
///     <para />
///     To dispatch an event,
///     <code>
///         EventManager.events.OnYourEvent()
///     </code>
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

    /// Player is invincible event
    public event Action OnPlayerInvincible;
    
    /// Player is vulnerable event
    public event Action OnPlayerVulnerable;
    
    /// Active/Deactivate Pause Menu
    public event Action<bool> OnPauseKeyDown;
    private bool _isPaused;
    
    /// Options screen cat is selected
    public event Action<uint> OnCatSelect;
    private uint _currentCatID;
    
    /// Fires when a Fox collides with the player
    public void FoxHitsPlayer()
    {
        OnFoxHitsPlayer?.Invoke();
    }

    /// Fires when the Player turns invincible
    public void PlayerInvincible()
    {
        OnPlayerInvincible?.Invoke();
        // TODO: Make single event, should not be a need for two
    }

    /// Fires when the player turns vulnerable
    public void PlayerVulnerable()
    {
        OnPlayerVulnerable?.Invoke();
    }
    
    /// <summary>Fires when a change in Pause status is detected</summary>
    /// <remarks>
    ///     If `isPaused` is true, Player just paused the game,
    ///     and logic of the paused state should be run.
    ///     If  `isPaused` is false, Player just unpaused the game,
    ///     and logic of the unpaused state should be run.
    /// </remarks>
    public void PauseKeyDown()
    {
        _isPaused ^= true;
        OnPauseKeyDown?.Invoke(_isPaused);
    }
    
    /// <summary>Fires when a cat is selected in the options menu</summary>
    public void CatSelect(uint index)
    {
        // Ignore if user selects the already active Cat selection
        if (index == _currentCatID) return;
        OnCatSelect?.Invoke(index);

        _currentCatID = index;
        
        var preferences = SaveSystem.LoadPreferences();
        preferences.CatID = index;
        SaveSystem.SavePreferences(preferences);
    }

}
