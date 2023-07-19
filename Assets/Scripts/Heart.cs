using System.Collections;
using UnityEngine;

/// <summary> Represents a heart game object in the game. </summary>
/// <remarks>
/// This class implements the IFlashable interface,
/// which allows hearts to exhibit a flashing effect when needed.
/// </remarks>
public class Heart : MonoBehaviour, IFlashable
{
    private SpriteRenderer sr;

    private void Awake()
    {
        this.sr = GetComponent<SpriteRenderer>();
    }

    public IEnumerator FlashEffect()
    {
        int flashCount = 3;
        float flickDuration = 0.3f;
        for (int i = 0; i < flashCount; i++)
        {
            // Make the sprite transparent
            sr.color = new Color(1f, 1f, 1f, 0.5f);
            yield return new WaitForSeconds(flickDuration);

            // Make the sprite opaque
            // ReSharper disable once Unity.InefficientPropertyAccess
            sr.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(flickDuration);
        }
    }

    /// Applies the Flash effect, then destroys the GameObject
    private IEnumerator DestroySelf()
    {
        yield return FlashEffect();
        Destroy(this.gameObject);
    }

    /// Animates and removes the Heart GameObject from the scene
    public void Destroy()
    {
        StartCoroutine(DestroySelf());
    }
}