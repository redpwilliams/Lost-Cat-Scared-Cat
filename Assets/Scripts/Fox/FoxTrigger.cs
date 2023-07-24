using UnityEngine;

public class FoxTrigger : MonoBehaviour
{

    private bool playerIsInvincible;

    private void OnEnable()
    {
        EventManager.Events.OnPlayerInvincible += PlayerIsInvincible;
        EventManager.Events.OnPlayerVulnerable += PlayerIsVulnerable;
    }

    private void OnDestroy()
    {
        EventManager.Events.OnPlayerInvincible -= PlayerIsInvincible;
        EventManager.Events.OnPlayerVulnerable -= PlayerIsVulnerable;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (this.playerIsInvincible || !col.CompareTag("Player")) return;
        EventManager.Events.FoxHitsPlayer();
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
