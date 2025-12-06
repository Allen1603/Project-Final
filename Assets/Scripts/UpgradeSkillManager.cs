using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSkillManager : MonoBehaviour
{
    [Header("References")]
    private PlayerController playerController;

    [Header("Upgrade Values")]
    public float stunIncrement = 0.5f;
    public float slowIncrement = 0.5f;
    public int healIncrement = 4;

    public static UpgradeSkillManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //if (skillManager == null) skillManager = SkillManager.instance;
        if (playerController == null) playerController = PlayerController.instance;
    }

    public void UpgradeSlow()
    {
        if (playerController == null) return;

        // Reset EXP to start from 0 again
        playerController.currentEXP = 0f;
        playerController.UpdateUIExp();

        // Apply the slow upgrade
        SkillManager.instance.NewSlowValue(slowIncrement);
    }

    public void UpgradeStun()
    {
        if (playerController == null) return;

        SkillManager.instance.NewStunValue(stunIncrement);
        playerController.currentEXP = 0f;
        playerController.UpdateUIExp();
    }

    public void UpgradeHeal()
    {
        if (playerController == null) return;

        SkillManager.instance.NewHealValue(healIncrement);
        playerController.currentEXP = 0f;
        playerController.UpdateUIExp();
    }

}
