using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    private void Start()
    {
        // Start menu BGM
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBGM("MainMenu");
        }
    }

    public void Play()
    {
        SceneManager.LoadSceneAsync(1);
        Time.timeScale = 1f;
    }

    public void Click()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ButtonClick");
        }
    }

    public void Tutorial()
    {
        SceneManager.LoadSceneAsync(3);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        SceneManager.LoadSceneAsync(0);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
