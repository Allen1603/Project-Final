using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileArrowButton : MonoBehaviour, IPointerDownHandler
{

    [HideInInspector] public bool wasTapped = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        wasTapped = true;
    }
}
