using UnityEngine;
using UnityEngine.UI;

public class AutoScrollWithTouch : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollSpeed = 0.05f;

    private bool isDragging = false;

    void Update()
    {
        if (isDragging) return;

        scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime;

        // When it reaches the bottom, reset to top
        if (scrollRect.verticalNormalizedPosition <= 0f)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }

    public void OnBeginDrag()
    {
        isDragging = true;
    }

    public void OnEndDrag()
    {
        isDragging = false;
    }
}
