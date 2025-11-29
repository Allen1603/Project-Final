using UnityEngine;

public class FrogAndInsectSelection : MonoBehaviour
{
    public GameObject[] pics;       // drag pic1, pic2, pic3, pic4 here
    public GameObject[] descriptions; // drag Idescrip1, Idescrip2, Idescrip3, Idescrip4 here

    void Start()
    {
        ShowCard(0); // show first card by default
    }

    // Call this from buttons: pass 0 for first card, 1 for second, etc.
    public void ShowCard(int index)
    {
        for (int i = 0; i < pics.Length; i++)
        {
            pics[i].SetActive(i == index);
            descriptions[i].SetActive(i == index);
        }
    }
}
