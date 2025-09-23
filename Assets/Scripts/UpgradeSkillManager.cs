using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSkillManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private PlayerController playerController;
    public GameObject skillUpgradePanel;

    [Header("Upgrade Values")]
    public float stunIncrement = 0.5f;
    public float slowIncrement = 0.5f;
    public int healIncrement = 4;

    private void Awake()
    {
        if (skillManager == null) skillManager = FindObjectOfType<SkillManager>();
        if (playerController == null) playerController = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        if (skillUpgradePanel != null)
            skillUpgradePanel.SetActive(false); // hide on start
    }

    public void UpgradeSlow()
    {
        if (skillManager == null || playerController == null) return;

        skillManager.NewSlowValue(slowIncrement);
        playerController.TakeExp(0);

        ClosePanel();
    }

    public void UpgradeStun()
    {
        if (skillManager == null || playerController == null) return;

        skillManager.NewStunValue(stunIncrement);
        playerController.TakeExp(0);

        ClosePanel();
    }

    public void UpgradeHeal()
    {
        if (skillManager == null || playerController == null) return;

        skillManager.NewHealValue(healIncrement);
        playerController.TakeExp(0);

        ClosePanel();
    }

    public void UpgradeClone()
    {
        if (skillManager == null || playerController == null) return;

        // Example: give clone duration or power upgrade
        //skillManager.NewCloneValue(1); // You can define logic inside SkillManager
        playerController.TakeExp(0);

        ClosePanel();
    }

    private void ClosePanel()
    {
        if (skillUpgradePanel != null)
            skillUpgradePanel.SetActive(false);
    }
}
