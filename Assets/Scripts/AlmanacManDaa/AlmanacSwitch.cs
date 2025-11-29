using UnityEngine;
using UnityEngine.UI;

public class AlmanacSwitch : MonoBehaviour
{
    public GameObject Frogs;
    public GameObject Insects;
    public GameObject ShowFrogs;
    public GameObject ShowInsects;
    public GameObject Almanac;

    void Start()
    {
        Frogs.gameObject.SetActive(true);
        Insects.gameObject.SetActive(false);
    }

    public void ShowFrogPanel()
    {
        Frogs.gameObject.SetActive(true);
        ShowFrogs.gameObject.SetActive(true);
        Insects.gameObject.SetActive(false);
        ShowInsects.gameObject.SetActive(false);
    }

    public void ShowInsectPanel()
    {
        Frogs.gameObject.SetActive(false);
        Insects.gameObject.SetActive(true);
        ShowInsects.gameObject.SetActive(true);
        ShowFrogs.gameObject.SetActive(false);
    }

    public void HideAlmanac()
    {
        Almanac.gameObject.SetActive(false);
    }
}
