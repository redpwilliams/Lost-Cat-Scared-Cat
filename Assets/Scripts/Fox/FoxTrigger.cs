using UnityEngine;

public class FoxTrigger : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Trigger initialized");
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Fox trigger activated");
    }
}
