using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    void Start()
    {
        if (AudioManager.Instance != null)
        {
            Debug.Log("Scene1Initializer: switching to BGM");
            AudioManager.Instance.StopBGM();
            AudioManager.Instance.PlayBGM("BGM");
        }
    }
}

