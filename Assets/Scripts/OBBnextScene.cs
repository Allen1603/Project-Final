using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OBBnextScene : MonoBehaviour
{
    public float delayBeforeLoad = 12f; // seconds before scene change
    public string nextSceneName = "MainMenu"; // name of the scene to load

    void Start()
    {
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoad);

        // Optional: You can insert OBB check here if needed

        SceneManager.LoadScene(nextSceneName);
    }
}
