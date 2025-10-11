using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Base : MonoBehaviour
{
    [Header("----- Base Settings -----")]
    public float maxLimit = 15f;
    public float currentLimit = 0;

    [Header("----- UI -----")]
    public TextMeshProUGUI limitText;
    public GameObject gameOverPanel; // Optional: assign in Inspector

    void Start()
    {
        currentLimit = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            currentLimit++;

            other.gameObject.SetActive(false);

            if (currentLimit >= maxLimit)
            {
                currentLimit = maxLimit;
                GameOver();
            }
        }
    }

    void GameOver()
    {
        Time.timeScale = 0f; // pause game

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}
