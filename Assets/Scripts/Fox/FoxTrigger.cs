using UnityEngine;

public class FoxTrigger : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D col)
    {
        // Debug.Log("Fox trigger activated: " + col.gameObject.name);
    }
}
