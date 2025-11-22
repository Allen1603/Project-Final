using UnityEngine;
using System.Collections;
using System;

public class SkillManager : MonoBehaviour
{
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
    public float maxHealValue = 20f;

    [Header("Clone Skill")]
    public GameObject clonePrefab;  // DRAG your CloneSkill prefab here
    public Transform clonePosition;

    private void Start()
    {

    }


    #region Stun Skill
    //----------------- STUN ---------------//

    public void ActivateStunField()
    {
        StartCoroutine(StunAllEnemies());
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

    public void ActivateSlowField()
    {
        StartCoroutine(SlowAllEnemies());
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
    }
    #endregion

    #region Clone Skill
    public void ActivateClone()
    {
        Instantiate(clonePrefab, clonePosition.position, Quaternion.identity);
    }
    #endregion

}
