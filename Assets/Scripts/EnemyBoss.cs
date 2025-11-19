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
    public int maxEnemies = 5;
    public int currentEnemies = 0;
    public float summonInterval = 3f;
    public float summonRadius = 3f;
    private float summonTimer;

    private List<GameObject> activeEnemies = new List<GameObject>();

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
        summonTimer += Time.deltaTime;

        // spawn only when it's time and under limit
        if (summonTimer >= summonInterval)
        {
            summonTimer = 0f;
            EnemySummon();
        }

        // check if any pooled enemies are inactive (returned to pool)
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (!activeEnemies[i].activeInHierarchy)
            {
                activeEnemies.RemoveAt(i);
            }
        }
    }

    private void EnemySummon()
    {
        // only spawn if under max limit
        if (activeEnemies.Count < maxEnemies)
        {
            Vector3 spawnPos = transform.position + (Vector3)(Random.insideUnitCircle * summonRadius);
            GameObject enemy = EnemyPool.Instance.SpawnFromPool("Enemy4", spawnPos, Quaternion.identity);

            if (enemy != null)
            {
                activeEnemies.Add(enemy);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Damage only from active Hook
        if (other.CompareTag("Hook"))
        {
            TDamage(tongueDamage);
        }

        if (other.CompareTag("Base"))
        {
            EnemyPool.Instance.ReturnToPool("Boss", gameObject);
        }
    }

    public void TDamage(float amount)
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
