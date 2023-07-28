using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // Search for an existing instance in the scene
            _instance = FindObjectOfType<T>();

            // If no instance exists, create a new one
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject(typeof(T).Name);
                _instance = singletonObject.AddComponent<T>();
            }

            // Keep the instance between scene changes
            DontDestroyOnLoad(_instance.gameObject);

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // Ensure there's only one instance of the singleton
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this as T;
        DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnDestroy()
    {
        // Clear the instance reference when the object is destroyed
        if (_instance == this)
        {
            _instance = null;
        }
    }
}