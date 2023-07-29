using System;

[Serializable]
public struct Preferences
{
    public Preferences(bool isFirstTime, uint catID)
    {
        if (catID > 5)
            throw new ArgumentOutOfRangeException(
                $"CatID {catID} is out of range of the maximum (5)");
        IsFirstTime = isFirstTime;
        CatID = catID;
    }

    /// Whether the Player is playing the game for the first time
    public bool IsFirstTime { get; set; }

    /// The Id of the Cat the Player wants to use (1-5, for Cats 1-5, respectively)
    public uint CatID { get; set; }

    /// ToString if needed
    public readonly override string ToString() => $"IsFirstTime: {IsFirstTime}, CatID: {CatID}";
}
