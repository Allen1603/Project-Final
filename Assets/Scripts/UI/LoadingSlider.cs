using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingSlider : MonoBehaviour
{
    [Header("Loading UI")]
    public Slider loadingSlider;

    [Header("Loading Speed Settings")]
    [Range(0.1f, 5f)]
    public float fillSpeed = 0.5f;

    [Header("Fade Transition Settings")]
    public CanvasGroup fadeCanvasGroup;  // Assign a UI panel with CanvasGroup
    public float fadeDuration = 1f;      // Crossfade speed

    void Start()
    {
        // Ensure fade starts invisible
        fadeCanvasGroup.alpha = 0f;

        StartCoroutine(LoadSceneSlow());
    }

    IEnumerator LoadSceneSlow()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(2);
        AudioManager.Instance.PlayBGM("BGM");
        operation.allowSceneActivation = false;

        float fakeProgress = 0f;

        while (!operation.isDone)
        {
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);

            fakeProgress = Mathf.MoveTowards(fakeProgress, realProgress, fillSpeed * Time.deltaTime);

            loadingSlider.value = fakeProgress;

            // When loading is 100%
            if (fakeProgress >= 1f)
            {
                // Start fade-out transition
                yield return StartCoroutine(Crossfade());

                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    IEnumerator Crossfade()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }
    }
}
