using UnityEngine;
using TMPro;
using System.Collections;

public class TMPFadeLoop : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public float fadeDuration = 1f;

    void Start()
    {
        StartCoroutine(FadeLoop());
    }

    IEnumerator FadeLoop()
    {
        while (true)
        {
            // Fade In
            yield return StartCoroutine(Fade(0f, 1f));

            // Fade Out
            yield return StartCoroutine(Fade(1f, 0f));
        }
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float time = 0f;
        Color color = tmpText.color;

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            tmpText.color = new Color(color.r, color.g, color.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        tmpText.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
