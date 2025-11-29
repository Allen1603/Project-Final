using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileArrowButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public bool isPressed = false;
    public Animator anim;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
