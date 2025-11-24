using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingSlider : MonoBehaviour
{
    public Slider loadingSlider;

    [Header("Loading Speed Settings")]
    [Range(0.1f, 5f)]             // Adjustable in Inspector
    public float fillSpeed = 0.5f; // Default speed

    void Start()
    {
        StartCoroutine(LoadSceneSlow());
    }

    IEnumerator LoadSceneSlow()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(2);
        operation.allowSceneActivation = false;

        float fakeProgress = 0f;

        while (!operation.isDone)
        {
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Slow down progress according to fillSpeed
            fakeProgress = Mathf.MoveTowards(fakeProgress, realProgress, fillSpeed * Time.deltaTime);

            loadingSlider.value = fakeProgress;

            if (fakeProgress >= 1f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
