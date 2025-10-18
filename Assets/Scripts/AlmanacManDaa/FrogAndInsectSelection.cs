using UnityEngine;
using UnityEngine.UI;

public class FrogAndInsectSelection : MonoBehaviour
{
    public GameObject pic1;
    public GameObject pic2;
    public GameObject pic3;


    public GameObject Idescrip1;
    public GameObject Idescrip2;
    public GameObject Idescrip3;


    void Start()
    {
        pic1.gameObject.SetActive(true);
        pic2.gameObject.SetActive(false);
        pic3.gameObject.SetActive(false);


        Idescrip1.gameObject.SetActive(true);
        Idescrip2.gameObject.SetActive(false);
        Idescrip3.gameObject.SetActive(false);
    }

    public void Card1()
    {
        pic1.gameObject.SetActive(true);
        pic2.gameObject.SetActive(false);
        pic3.gameObject.SetActive(false);

        Idescrip1.gameObject.SetActive(true);
        Idescrip2.gameObject.SetActive(false);
        Idescrip3.gameObject.SetActive(false);
    }
    public void Card2()
    {
        pic1.gameObject.SetActive(false);
        pic2.gameObject.SetActive(true);
        pic3.gameObject.SetActive(false);

        Idescrip1.gameObject.SetActive(false);
        Idescrip2.gameObject.SetActive(true);
        Idescrip3.gameObject.SetActive(false);
    }
    public void Card3()
    {
        pic1.gameObject.SetActive(false);
        pic2.gameObject.SetActive(false);
        pic3.gameObject.SetActive(true);

        Idescrip1.gameObject.SetActive(false);
        Idescrip2.gameObject.SetActive(false);
        Idescrip3.gameObject.SetActive(true);
    }

}
