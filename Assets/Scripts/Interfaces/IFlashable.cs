using System.Collections;

/// <summary>
/// Represents an object that can perform a flash effect.
/// </summary>
public interface IFlashable
{
    /// <summary>
    /// Plays a flash effect on the object.
    /// </summary>
    /// <remarks>
    /// The flash effect involves making the object temporarily transparent
    /// or to provide a visual cue.<para />
    /// The time it will take for the entire flash animation to end is found by:
    /// <code>2 * flickDuration * flashCount</code>
    /// </remarks>
    /// <returns>An enumerator for the flash effect coroutine.</returns>
    // ReSharper disable once UnusedMemberInSuper.Global
    IEnumerator FlashEffect();
}
