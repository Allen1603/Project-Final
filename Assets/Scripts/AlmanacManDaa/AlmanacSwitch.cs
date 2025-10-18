using UnityEngine;
using UnityEngine.UI;

public class AlmanacSwitch : MonoBehaviour
{
    public GameObject Frogs;
    public GameObject Insects;
    public GameObject Almanac;

    void Start()
    {
        Frogs.gameObject.SetActive(true);
        Insects.gameObject.SetActive(false);
    }

    public void ShowFrogPanel()
    {
        Frogs.gameObject.SetActive(true);
        Insects.gameObject.SetActive(false);
    }

    public void ShowInsectPanel()
    {
        Frogs.gameObject.SetActive(false);
        Insects.gameObject.SetActive(true);
    }

    public void HideAlmanac()
    {
        Almanac.gameObject.SetActive(false);
    }
}
