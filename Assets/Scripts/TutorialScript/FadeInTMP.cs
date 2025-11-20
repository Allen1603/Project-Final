using System.Collections;
using UnityEngine;
using TMPro;

public class FadeInTMP : MonoBehaviour
{
    [Header("Fade Settings")]
    [Tooltip("How long the fade-in takes in seconds.")]
    public float fadeDuration = 1.5f;

    private TMP_Text tmpText;
    private Color originalColor;

    void Awake()
    {
        tmpText = GetComponent<TMP_Text>();

        originalColor = tmpText.color;

        // Start fully transparent
        tmpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            float alpha = timer / fadeDuration;
            tmpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        // Ensure fully visible
        tmpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
    }
}
