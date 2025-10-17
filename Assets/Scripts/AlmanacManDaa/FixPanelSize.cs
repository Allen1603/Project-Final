using UnityEngine;
using UnityEngine.UI;

public class FixPanelSize : MonoBehaviour
{
    public RectTransform viewport;
    public RectTransform content;

    void Start()
    {
        float width = viewport.rect.width;
        float height = viewport.rect.height;

        foreach (RectTransform panel in content)
        {
            panel.sizeDelta = new Vector2(width, height);
            panel.anchoredPosition3D = Vector3.zero;
        }
    }
}
