using UnityEngine;

public class FoxTrigger : MonoBehaviour
{

    private bool playerIsInvincible;

    private void OnEnable()
    {
        EventManager.events.OnPlayerInvincible += PlayerIsInvincible;
        EventManager.events.OnPlayerVulnerable += PlayerIsVulnerable;
    }

    private void OnDestroy()
    {
        EventManager.events.OnPlayerInvincible -= PlayerIsInvincible;
        EventManager.events.OnPlayerVulnerable -= PlayerIsVulnerable;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (this.playerIsInvincible || !col.CompareTag("Player")) return;
        EventManager.events.FoxHitsPlayer();
    }

    private void PlayerIsInvincible()
    {
        this.playerIsInvincible = true;
    }

    private void PlayerIsVulnerable()
    {
        this.playerIsInvincible = false;
    }
}
