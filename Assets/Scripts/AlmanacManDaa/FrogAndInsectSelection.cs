using UnityEngine;
using UnityEngine.UI;

public class FrogAndInsectSelection : MonoBehaviour
{
    public GameObject[] pics;          // assign pic1, pic2, pic3, pic4
    public GameObject[] descriptions;  // assign Idescrip1, Idescrip2, Idescrip3, Idescrip4
    public Button[] buttons;           // assign buttons corresponding to each card

    public Color selectedColor = new Color(0f, 0f, 0f, 0.5f); // dark & semi-transparent
    public Color normalColor = new Color(1f, 1f, 1f, 1f);     // fully opaque white

    void Start()
    {
        ShowCard(0); // show first card by default
    }

    public void ShowCard(int index)
    {
        for (int i = 0; i < pics.Length; i++)
        {
            // Activate selected card and description
            pics[i].SetActive(i == index);
            descriptions[i].SetActive(i == index);

            // Change button color
            if (buttons != null && buttons.Length > i)
            {
                buttons[i].GetComponent<Image>().color = (i == index) ? selectedColor : normalColor;
            }
        }
    }
}
