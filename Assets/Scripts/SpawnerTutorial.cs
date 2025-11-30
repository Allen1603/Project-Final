using System.Collections;
using UnityEngine;

public class SpawnerTutorial : MonoBehaviour
{
    [Header("----- Enemy Prefabs & Tags -----")]
    public string[] enemyTags;  // 4 enemy types

    [Header("----- Spawner Points -----")]
    public Transform[] spawner;

    [Header("----- Spawn Settings -----")]
    public float spawnInterval = 3f;
    public float spawnDecrement = 0.2f;
    public float minInterval = 1f;

    [Header("----- Wave Settings -----")]
    public int startingEnemiesPerWave = 1;   // fixed number per wave

    private int currentWave = 1;
    private int enemiesSpawnedThisWave;

    public GameObject exitPanel;

    private void Start()
    {
        exitPanel.SetActive(false);
    }

    private IEnumerator InitializeSpawner()
    {
        yield return null;
        StartWave();
    }

    private void StartWave()
    {
        enemiesSpawnedThisWave = 0;
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        while (enemiesSpawnedThisWave < startingEnemiesPerWave)
        {
            SpawnEnemy();
            enemiesSpawnedThisWave++;
            yield return new WaitForSeconds(spawnInterval);
        }

        currentWave++;

        // 🔥 Show exit panel at wave 4
        if (currentWave == 6)
        {
            exitPanel.SetActive(true);
        }

        // decrease interval safely
        spawnInterval = Mathf.Clamp(spawnInterval - spawnDecrement, minInterval, 999f);

        yield return new WaitForSeconds(1.5f);
        StartWave();
    }

    private void SpawnEnemy()
    {
        // Choose spawn point
        Transform spawnPoint = spawner[Random.Range(0, spawner.Length)];

        // Choose enemy type based on wave (1→0,2→1,3→2,4+→3)
        int index = Mathf.Clamp(currentWave - 1, 0, enemyTags.Length - 1);

        string enemyTag = enemyTags[index];

        EnemyPool.Instance.SpawnFromPool(enemyTag, spawnPoint.position, Quaternion.identity);
    }

    public void StartWaveSpawn()
    {
        StartCoroutine(InitializeSpawner());
    }
}
