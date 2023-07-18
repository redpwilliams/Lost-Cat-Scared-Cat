using UnityEngine;

public class FoxTrigger : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name != "Player") return;
        EventManager.events.OnFoxHitsPlayer();
    }
}
