using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Zoom Settings")]
    public float zoomScale = 1.2f;
    public float zoomSpeed = 10f;

    private Vector3 originalScale;
    private Vector3 targetScale;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    void OnEnable()
    {
        // Reset instantly when the button resets or the canvas opens
        transform.localScale = originalScale;
        targetScale = originalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.unscaledDeltaTime * zoomSpeed
        );
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * zoomScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
}
