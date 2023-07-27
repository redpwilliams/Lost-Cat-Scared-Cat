using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializationManager : MonoBehaviour
{
    /// Time it takes for the Title to appear
    [SerializeField] private float titleDelay = 2f;
    [SerializeField] private GameObject title;
    
    
    /// Time it takes for the Author to appear after the title
    [Space(10)]
    [SerializeField] private float authorDelay = 2f;
    [SerializeField] private GameObject author;
    
    /// Time it takes for the Unity logo to appear after the author
    [Space(10)]
    [SerializeField] private float logoDelay = 2f;
    [SerializeField] private GameObject logo;

    /// Additional time it takes before scene is loaded
    [Space(10)]
    [SerializeField] private float sceneDelay = 3f;
    
    private void Awake()
    {
        var preferences = SaveSystem.LoadPreferences();
        Debug.Log(preferences);
        StartCoroutine(AnimateFrontMatter());
    }

    private IEnumerator AnimateFrontMatter()
    {
        yield return new WaitForSeconds(this.titleDelay);
        this.title.SetActive(true);

        yield return new WaitForSeconds(this.authorDelay);
        this.author.SetActive(true);

        yield return new WaitForSeconds(this.logoDelay);
        this.logo.SetActive(true);

        yield return new WaitForSeconds(this.sceneDelay);
        SceneManager.LoadScene("Scenes/Title Screen");
    }
}
