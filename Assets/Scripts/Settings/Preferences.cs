
[System.Serializable]
public readonly struct Preferences
{

    public Preferences(bool isFirstTime, uint catID)
    {
        IsFirstTime = isFirstTime;
        CatID = catID;
    }

    /// Whether the Player is playing the game for the first time
    public bool IsFirstTime { get; }

    /// The Id of the Cat the Player wants to use (0-4, for Cats 1-5, respectively)
    public uint CatID { get; }

    /// ToString if needed
    public readonly override string ToString() => $"[{IsFirstTime}], [{CatID}]";
}
