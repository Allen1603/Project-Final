using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingSlider : MonoBehaviour
{
    [Header("Loading UI")]
    public Slider loadingSlider;

    [Header("Object to Follow Slider")]
    public Transform followObject;          // GameObject you want to move
    public RectTransform sliderFillArea;    // The bar of the slider for position reference

    [Header("Loading Speed Settings")]
    [Range(0.1f, 5f)]
    public float fillSpeed = 0.5f;

    [Header("Fade Transition Settings")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    void Start()
    {
        fadeCanvasGroup.alpha = 0f;
        StartCoroutine(LoadSceneSlow());
    }

    IEnumerator LoadSceneSlow()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("MapPicking");
        AudioManager.Instance.PlayBGM("BGM");
        operation.allowSceneActivation = false;

        float fakeProgress = 0f;

        while (!operation.isDone)
        {
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);

            fakeProgress = Mathf.MoveTowards(fakeProgress, realProgress, fillSpeed * Time.deltaTime);

            loadingSlider.value = fakeProgress;

            // ---------------------------------
            // Move game object along the slider
            // ---------------------------------
            if (followObject != null && sliderFillArea != null)
            {
                float xMin = sliderFillArea.position.x - (sliderFillArea.rect.width * 0.5f);
                float xMax = sliderFillArea.position.x + (sliderFillArea.rect.width * 0.5f);

                float xPos = Mathf.Lerp(xMin, xMax, loadingSlider.value);

                followObject.position = new Vector3(xPos, followObject.position.y, followObject.position.z);
            }

            if (fakeProgress >= 1f)
            {
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
