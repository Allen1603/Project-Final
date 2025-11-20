using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeLoop : MonoBehaviour
{
    [Header("Settings")]
    public float fadeDuration = 1f; 
    public bool loop = true;       

    [Header("References")]
    public Graphic uiElement;       

    private void Start()
    {
        if (uiElement == null)
        {
            uiElement = GetComponent<Graphic>();
        }

        if (uiElement != null)
            StartCoroutine(FadeCoroutine());
    }

    private System.Collections.IEnumerator FadeCoroutine()
    {
        while (loop)
        {
            // Fade In
            yield return StartCoroutine(Fade(0f, 1f));

            // Fade Out
            yield return StartCoroutine(Fade(1f, 0f));
        }
    }

    private System.Collections.IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = uiElement.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            uiElement.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Ensure final alpha
        uiElement.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
