using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBGM("MainMenu");
        }
    }

    public void Play()
    {
        SceneManager.LoadSceneAsync(2);
        Time.timeScale = 1f;
    }


    public void Click()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ButtonClick");
        }
    }

    public void AlmanacBackButton()
    {
        SceneManager.LoadSceneAsync("MainMenu");
        Time.timeScale = 1f;
        AudioManager.Instance.PlaySFX("ButtonClick");

    }

    public void ClickFroggy()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("froggy");
        }
    }

    public void Almanac()
    {
        // Play SFX
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("froggy");

        // Load Almanac Scene
        SceneManager.LoadSceneAsync(7);
        Time.timeScale = 1f;
    }

    public void Tutorial()
    {
        SceneManager.LoadSceneAsync(6);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        SceneManager.LoadSceneAsync(1);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
