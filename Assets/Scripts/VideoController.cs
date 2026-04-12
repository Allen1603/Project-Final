using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button watchAgainButton;

    void Start()
    {
        // hide button at start
        watchAgainButton.gameObject.SetActive(false);

        // play video
        videoPlayer.Play();

        // listen for video end
        videoPlayer.loopPointReached += OnVideoEnd;

        // hook up button
        watchAgainButton.onClick.AddListener(WatchAgain);
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // show button when video finishes
        watchAgainButton.gameObject.SetActive(true);
    }

    public void WatchAgain()
    {
        // hide button again
        watchAgainButton.gameObject.SetActive(false);

        // restart video from beginning
        videoPlayer.time = 0;
        videoPlayer.Play();
    }
}
