using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWriterTMP : MonoBehaviour
{
    [Header("Settings")]
    public float typingSpeed = 0.05f;   // Delay between each character
    public bool playOnStart = true;

    [Header("References")]
    public TextMeshProUGUI tmpText;

    private string fullText;

    private void Start()
    {
        if (tmpText == null)
            tmpText = GetComponent<TextMeshProUGUI>();

        fullText = tmpText.text;
        tmpText.text = "";

        if (playOnStart)
            StartTypewriter();
    }

    public void StartTypewriter()
    {
        StopAllCoroutines();
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        tmpText.text = "";

        foreach (char c in fullText)
        {
            tmpText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
