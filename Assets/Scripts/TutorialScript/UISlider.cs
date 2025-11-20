using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform content;  // The parent of images
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;
    [SerializeField] private GameObject exitButton;

    [Header("Settings")]
    [SerializeField] private float slideDuration = 0.5f; // seconds
    [SerializeField] private float spacing = 0f; // add if you have gaps in layout

    private int currentIndex = 0;
    private int totalImages;
    private float imageWidth;
    private bool isSliding = false;

    private void Start()
    {
        totalImages = content.childCount;
        if (totalImages > 0)
        {
            // width of one child (assuming equal sizes)
            imageWidth = ((RectTransform)content.GetChild(0)).rect.width + spacing;
        }

        nextButton.onClick.AddListener(Next);
        prevButton.onClick.AddListener(Previous);
        UpdateButtonState();
    }

    private void Next()
    {
        if (isSliding || currentIndex >= totalImages - 1) return;
        currentIndex++;
        StartCoroutine(SlideToIndex(currentIndex));
    }

    private void Previous()
    {
        if (isSliding || currentIndex <= 0) return;
        currentIndex--;
        StartCoroutine(SlideToIndex(currentIndex));
    }

    private IEnumerator SlideToIndex(int index)
    {
        isSliding = true;
        Vector2 startPos = content.anchoredPosition;
        Vector2 targetPos = new Vector2(-index * imageWidth, startPos.y);

        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            float t = elapsed / slideDuration;
            t = Mathf.SmoothStep(0, 1, t);
            content.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        content.anchoredPosition = targetPos;
        isSliding = false;
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        prevButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < totalImages - 1;

        if (currentIndex == totalImages - 1)
            exitButton.SetActive(true);
    }

    public void EnableCanvas()
    {
        gameObject.SetActive(true);
    }
}
