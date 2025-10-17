using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SwipeSnap : MonoBehaviour, IEndDragHandler
{
    public ScrollRect scrollRect;
    public int totalPages = 3; // number of panels
    private float[] positions;
    private int currentPage = 0;

    void Start()
    {
        positions = new float[totalPages];
        for (int i = 0; i < totalPages; i++)
            positions[i] = (float)i / (totalPages - 1);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float pos = scrollRect.horizontalNormalizedPosition;
        int targetPage = Mathf.RoundToInt(pos * (totalPages - 1));
        StartCoroutine(SmoothMove(positions[targetPage]));
        currentPage = targetPage;
    }

    private System.Collections.IEnumerator SmoothMove(float target)
    {
        float start = scrollRect.horizontalNormalizedPosition;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 5;
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(start, target, t);
            yield return null;
        }
    }
}
