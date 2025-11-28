using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene3Initializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(AudioManager.Instance != null)
        {
            Debug.Log("Scene3Initializer: switching to BGM");
            AudioManager.Instance.StopBGM();
            AudioManager.Instance.PlayBGM("BGM");
        }
    }

   
}
