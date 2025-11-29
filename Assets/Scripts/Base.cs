using UnityEngine;
using TMPro;

public class Base : MonoBehaviour
{
    [Header("----- Base Settings -----")]
    public float maxLimit = 15f;
    public float currentLimit = 0;

    [Header("----- UI -----")]
    public TextMeshProUGUI limitText;

    void Start()
    {
        currentLimit = 0;
        UpdateLimitText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy1") || other.CompareTag("Enemy2") || other.CompareTag("Enemy3") || other.CompareTag("Enemy4") || other.CompareTag("Boss"))
        {
            currentLimit++;

            if (currentLimit >= maxLimit)
                currentLimit = maxLimit;

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
