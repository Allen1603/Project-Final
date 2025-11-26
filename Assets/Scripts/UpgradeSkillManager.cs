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

    [Header("Upgrade System")]
    public int cloneLevel = 1; // 1-4
    //private int maxTargets = 1; // level 4 allows 2 enemies
    //private float consumeSpeedMultiplier = 1f; // Level 2 bonus
    //private float cooldownModifier = 0f;       // Level 3 bonus

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

    public void UpgradeClone()
    {
        if (playerController == null) return;

        // Increase clone level (max 4)
        if (cloneLevel < 4)
            cloneLevel++;

        // Reset player EXP
        playerController.currentEXP = 0f;
        playerController.UpdateUIExp();

        // Update all active or future clones (optional if using prefabs)
        CloneSkill[] activeClones = FindObjectsOfType<CloneSkill>();
        foreach (var clone in activeClones)
        {
            clone.cloneLevel = cloneLevel;
            clone.ApplyUpgrades(); // make ApplyUpgrades public in CloneSkill
        }
    }
}
