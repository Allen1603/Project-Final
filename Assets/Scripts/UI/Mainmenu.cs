using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Tutorial()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
