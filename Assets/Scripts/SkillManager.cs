using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [Header("Skill Button Effects")]
    public GameObject stunEffect;
    public GameObject slowEffect;
    public GameObject healEffect;
    public GameObject cloneEffect;

    [Header("Stun Skill")]
    public float baseStunDuration = 1f;
    public float currentStunValue = 0f;
    public float maxStunValue = 5f;

    [Header("Slow Skill")]
    public float baseSlowDuration = 1f;
    public float newSlowSpeed = 0.5f;
    public float currentSlowValue = 0f;
    public float maxSlowValue = 5f;

    [Header("Heal Skill")]
    public int baseHealAmount = 10;
    public int currentHealValue = 0;
    public int maxHealValue = 20;

    [Header("Clone Skill")]
    public GameObject clonePrefab;  // DRAG your CloneSkill prefab here
    public Transform clonePosition;
    private FrogData selectedFrog;

    [Header("Skill Button")]
    public Button stunButton;
    public Button slowButton;
    public Button healButton;
    public Button cloneButton;

    public static SkillManager instance;
    private PlayerController player;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player = PlayerController.instance;
        UpdateSkillButtons(0, player.MaxBar);

        selectedFrog = FrogSelectionManager.instance.selectedFrog;

        // Replace the default clonePrefab with the selected frog's clonePrefab
        clonePrefab = selectedFrog.clonePrefab;
    }

    #region Stun Skill
    //----------------- STUN ---------------//
    public void NewStunValue(float addedDuration)
    {
        currentStunValue += addedDuration;
        currentStunValue = Mathf.Clamp(currentStunValue, 0f, maxStunValue);
    }

    public void ActivateStunField()
    {
        StartCoroutine(StunAllEnemies());
        ResetBar();
    }

    IEnumerator StunAllEnemies()
    {
        EnemyFly[] flies = FindObjectsOfType<EnemyFly>();
        EnemyBee[] bees = FindObjectsOfType<EnemyBee>();
        EnemyBug[] bugs = FindObjectsOfType<EnemyBug>();
        EnemyHopper[] hoppers = FindObjectsOfType<EnemyHopper>();

        foreach (EnemyFly enemy in flies)
            enemy.Stun(baseStunDuration);

        foreach (EnemyBee enemy in bees)
            enemy.Stun(baseStunDuration);

        foreach (EnemyBug enemy in bugs)
            enemy.Stun(baseStunDuration);

        foreach (EnemyHopper enemy in hoppers)
            enemy.Stun(baseStunDuration);

        yield return null;
    }
    #endregion

    #region Slow Skill
    //----------------- SLOW ---------------//
    public void NewSlowValue(float addedDuration)
    {
        currentSlowValue += addedDuration;
        currentSlowValue = Mathf.Clamp(currentSlowValue, 0f, maxSlowValue);
    }
    public void ActivateSlowField()
    {
        StartCoroutine(SlowAllEnemies());
        ResetBar();
    }

    IEnumerator SlowAllEnemies()
    {
        float duration = currentSlowValue > 0 ? currentSlowValue : baseSlowDuration;

        EnemyFly[] flies = FindObjectsOfType<EnemyFly>();
        EnemyBug[] bugs = FindObjectsOfType<EnemyBug>();
        EnemyHopper[] hoppers = FindObjectsOfType<EnemyHopper>();
        EnemyBee[] bees = FindObjectsOfType<EnemyBee>();

        foreach (EnemyFly fly in flies)
            fly.SlowEffect(newSlowSpeed, duration);

        foreach (EnemyBee bee in bees)
            bee.SlowEffect(newSlowSpeed, duration);

        foreach (EnemyBug bug in bugs)
            bug.SlowEffect(newSlowSpeed, duration);

        foreach (EnemyHopper hopper in hoppers)
            hopper.SlowEffect(newSlowSpeed, duration);

        yield return null;
    }
    #endregion

    #region Heal Skill
    public void NewHealValue(int newHealValue)
    {
        currentHealValue += newHealValue;
        currentHealValue = Mathf.Clamp(currentHealValue, 0, maxHealValue);
    }
    public void HealPlayer()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            int bonusHeal = currentHealValue + baseHealAmount;
            player.Heal(bonusHeal);
        }
        else
        {
            Debug.LogWarning("No PlayerController found!");
        }

        ResetBar();
    }
    #endregion

    #region Clone Skill
    public void ActivateClone()
    {
        Instantiate(clonePrefab, clonePosition.position, Quaternion.identity);
        ResetBar();
    }

    public void UpdateSkillButtons(float currentBar, float maxBar)
    {
        bool isFull = currentBar >= maxBar;

        if (stunButton != null) stunButton.interactable = isFull;
        if (slowButton != null) slowButton.interactable = isFull;
        if (healButton != null) healButton.interactable = isFull;
        if (cloneButton != null) cloneButton.interactable = isFull;

        if (stunEffect != null) stunEffect.SetActive(isFull);
        if (slowEffect != null) slowEffect.SetActive(isFull);
        if (healEffect != null) healEffect.SetActive(isFull);
        if (cloneEffect != null) cloneEffect.SetActive(isFull);
    }

    public void ResetBar()
    {
        if (player != null)
        {
            player.currentBar = 0;
            player.UpdateUIBar();
            UpdateSkillButtons(0, player.MaxBar);
        }
    }
    #endregion

}
