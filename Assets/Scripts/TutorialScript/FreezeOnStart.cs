using UnityEngine;

public class FreezeOnStart : MonoBehaviour
{
    [Header("Tutorial Settings")]
    public bool freezeOnStart = true;   // Auto freeze on game start
    public GameObject tutorialUI;       // Optional: show tutorial UI

    void Start()
    {
        if (freezeOnStart)
        {
            FreezeGame();
        }
    }

    public void FreezeGame()
    {
        Time.timeScale = 0f; // Freeze everything
        if (tutorialUI != null)
            tutorialUI.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume normal gameplay
        if (tutorialUI != null)
            tutorialUI.SetActive(false);
    }
}
