using UnityEngine;
using TMPro;

public class EggManager : MonoBehaviour
{
    private int eggsAlive;
    public int maxLimit = 50;
    public TextMeshProUGUI limitText;

    private void Start()
    {
        EggHealth[] eggs = FindObjectsOfType<EggHealth>();
        eggsAlive = eggs.Length;

        // assign manager reference to each egg
        foreach (EggHealth egg in eggs)
        {
            egg.manager = this;
        }

        UpdateLimitText();
    }

    public void EggDied()
    {
        eggsAlive--;

        UpdateLimitText();

        if (eggsAlive <= 0)
        {
            if (GameOverManager.instance != null)
                GameOverManager.instance.ShowGameOver();
        }
    }
    private void UpdateLimitText()
    {
        limitText.text = eggsAlive + " / " + maxLimit;
    }
}
