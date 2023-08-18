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
        
        // Set cat id
        _currentCatID = SaveSystem.LoadPreferences().CatID;
    }

    /// Game has officially started event
    public event Action OnPlayStart;
    private bool _gameHasStarted;

    /// When a new tutorial skulk starts
    public event Action OnNewTutorialSkulk;

    /// When all tutorial skulks are finished
    public event Action OnEndTutorialSkulks;

    /// Fox hits player event
    public event Action OnFoxHitsPlayer;

    /// Player is invincible event
    public event Action OnPlayerInvincible;
    
    /// Player is vulnerable event
    public event Action OnPlayerVulnerable;
    
    /// Active/Deactivate Pause Menu
    public event Action<bool> OnPauseKeyDown;
    private bool _isPaused;

    /// Fires when the Player loses last heart and game is over
    public event Action OnGameOver;
    internal int FinalScore { get; private set; }

    /// Options screen cat is selected
    public event Action<uint> OnCatSelect;
    private uint _currentCatID;

    /// Fires when the Player touches the screen and the game/scroll begins
    public void PlayStart()
    {
        OnPlayStart?.Invoke();
    }

    /// Fires when a tutorial Skulk starts
    public void NewTutorialSkulk()
    {
        OnNewTutorialSkulk?.Invoke();
    }

    /// Fires when the tutorial section is completed
    public void EndTutorialSkulks()
    {
        OnEndTutorialSkulks?.Invoke();
    }
    
    /// Fires when a Fox collides with the player
    public void FoxHitsPlayer()
    {
        OnFoxHitsPlayer?.Invoke();
    }

    /// Fires when the Player turns invincible
    public void PlayerInvincible()
    {
        OnPlayerInvincible?.Invoke();
        // REVIEW: Make single event, should not be a need for two
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

    public void GameOver(float score)
    {
        FinalScore = Mathf.RoundToInt(score);
        var prefs = SaveSystem.LoadPreferences();
        if (prefs.HighScore < FinalScore)
        {
            prefs.HighScore = FinalScore;
            SaveSystem.SavePreferences(prefs);
        }
        
        OnGameOver?.Invoke();
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
