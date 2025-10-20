using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1.5f;

    [Header("Summoning")]
    public GameObject[] enemyPrefabs;
    public string[] enemyTags; // must match EnemyPool tags
    public int enemySummon = 5;
    public float summonInterval = 3f;
    public float summonRadius = 3f;
    private float summonTimer;

    [Header("Health")]
    public float BossHealth = 300f;
    private float currentBossHealth;

    [Header("Combat")]
    public float tongueDamage = 20f;
    public bool isHooked = false;

    private void OnEnable()
    {
        isHooked = false;
        currentBossHealth = BossHealth;
        summonTimer = summonInterval;
    }

    private void Update()
    {
        if (isHooked) return;

        // Movement (keeps Y constant)
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // Summon countdown
        summonTimer -= Time.deltaTime;
        if (summonTimer <= 0f)
        {
            EnemySummon();
            summonTimer = summonInterval;
        }
    }

    private void EnemySummon()
    {
        if (EnemyPool.Instance == null) return;

        for (int i = 0; i < enemySummon; i++)
        {
            Vector3 spawnPos = transform.position + (Vector3)(Random.insideUnitCircle * summonRadius);
            EnemyPool.Instance.SpawnFromPool("Enemy4", transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Damage only from active Hook
        if (other.CompareTag("Hook"))
        {
            TakeDamage(tongueDamage);
        }

        if (other.CompareTag("Base"))
        {
            EnemyPool.Instance.ReturnToPool("Boss", gameObject);
        }
    }

    public void TakeDamage(float amount)
    {
        currentBossHealth -= amount;
        Debug.LogError("Boss damaged! Current HP: " + currentBossHealth);

        if (currentBossHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isHooked = true;
        PlayerController.instance.TakeBar(100f);
        PlayerController.instance.TakeExp(100f);

        // Return to pool safely
        EnemyPool.Instance.ReturnToPool("Boss", gameObject);
    }
}
