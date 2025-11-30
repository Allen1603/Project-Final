using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AlmanacSwitch : MonoBehaviour
{
    public GameObject Frogs;
    public GameObject Insects;
    public GameObject ShowFrogs;
    public GameObject ShowInsects;
    public GameObject Almanac;

    void Start()
    {
        // Play Almanac BGM when the scene loads
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayBGM("AlmanacBGM");

        // Default panels
        Frogs.SetActive(true);
        Insects.SetActive(false);
    }

    public void ShowFrogPanel()
    {
        Frogs.SetActive(true);
        ShowFrogs.SetActive(true);
        Insects.SetActive(false);
        ShowInsects.SetActive(false);
    }

    public void ShowInsectPanel()
    {
        Frogs.SetActive(false);
        Insects.SetActive(true);
        ShowInsects.SetActive(true);
        ShowFrogs.SetActive(false);
    }

    public void HideAlmanac()
    {
        Almanac.SetActive(false);
    }

    public void ReturnMainMenu()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("ButtonClick");

        SceneManager.LoadSceneAsync("MainMenu");
        Time.timeScale = 1f;
    }

    public void ButtonClickSFX()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("ButtonClick");
    }
}
