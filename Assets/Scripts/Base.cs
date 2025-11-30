using UnityEngine;
using TMPro;

public class Base : MonoBehaviour
{
    [Header("----- Base Settings -----")]
    public float maxLimit = 50f;
    public float currentLimit = 50f;

    [Header("----- UI -----")]
    public TextMeshProUGUI limitText;

    void Start()
    {
        currentLimit = 50;
        UpdateLimitText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy1") || other.CompareTag("Enemy2") || other.CompareTag("Enemy3") || other.CompareTag("Enemy4") || other.CompareTag("Boss"))
        {
            currentLimit--;

            if (currentLimit <= 0)
            {
                currentLimit = 0;

                if (GameOverManager.instance != null)
                    GameOverManager.instance.ShowGameOver();
            }
            UpdateLimitText();
            // Hide or return to pool
            other.gameObject.SetActive(false);
        }
    }

    private void UpdateLimitText()
    {
        limitText.text = currentLimit + " / " + maxLimit;
    }
}
