using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInitializer : MonoBehaviour
{
    void Start()
    {
        if (AudioManager.Instance == null) return;

        string sceneName = SceneManager.GetActiveScene().name;

        AudioManager.Instance.StopBGM(); // stop previous BGM

        if (sceneName == "Almanac")
            AudioManager.Instance.PlayBGM("AlmanacBGM");
        else if (sceneName == "MainMenu")
            AudioManager.Instance.PlayBGM("MainMenu");
    }
}
