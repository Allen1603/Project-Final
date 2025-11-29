using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OBBnextScene : MonoBehaviour
{
    public float delayBeforeLoad = 12f;
    public string nextSceneName = "MainMenu";

    [Header("Crossfade Settings")]
    public RawImage fadeImage;
    public float fadeDuration = 1f;

    void Start()
    {
        StartCoroutine(LoadNextSceneWithFade());
    }

    IEnumerator LoadNextSceneWithFade()
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator FadeOut()
    {
        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
    }
}
