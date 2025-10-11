using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBoss : MonoBehaviour
{
    public float moveSpeed;
    public GameObject[] enemyPrefabs;
    public string[] enemyTags; // tags must match EnemyPool tags
    public int enemySummon = 5;
    public float summonInterval = 3f;
    public float summonRadius = 3f;
    private float summonTimer;

    public float currentBossHealth = 300f;

    void Start()
    {
        summonTimer = summonInterval;
    }

    void Update()
    {
        // Move boss horizontally only
        float fixedYPosition = transform.position.y;
        transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, fixedYPosition, transform.position.z);

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
            // Pick a random enemy type
            int randomIndex = Random.Range(0, enemyTags.Length);
            string tag = enemyTags[randomIndex];

            // Random position around the boss
            Vector3 spawnPos = transform.position + (Vector3)(Random.insideUnitCircle * summonRadius);

            // Get enemy from pool (no Instantiate = no lag)
            EnemyPool.Instance.SpawnFromPool("Boss", transform.position, Quaternion.identity);
        }
    }
}
