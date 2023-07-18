using UnityEngine;
using Character = UnityEngine.TextCore.Text.Character;

public class FoxTrigger : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        EventManager.events.OnFoxHitsPlayer();
    }
}
