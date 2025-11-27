using System.Collections;
using UnityEngine;
using TMPro;

public class SpawnerEnemy : MonoBehaviour
{
    [Header("----- Enemy Prefabs & Tags -----")]
    [Tooltip("Enemy tags that exist in EnemyPool (ex: Bug1, Bug2, Bug3, Bug4)")]
    public string[] enemyTags; // should have 4 tags here

    [Header("----- Spawner Points -----")]
    public Transform[] spawner; // assign multiple spawn points in inspector
    public Transform rightSpawner;
    public Transform[] leftSpawner;

    [Header("----- Spawn Settings -----")]
    public float spawnInterval = 3f;     // initial time between spawns
    public float spawnDecrement = 0.2f;  // reduces spawn interval per wave
    public float limitDecrement = 1f;    // min allowed interval

    [Header("----- Wave Settings -----")]
    public int startingEnemiesPerWave = 7;
    public int enemyIncrementPerWave = 3;
    private bool bossActivate = false;

    private int currentWave = 1;
    private int enemiesToSpawnInWave;
    private int enemiesSpawnedThisWave;

    [Header("----- Wave UI -----")]
    public GameObject wavePanel;
    public TextMeshProUGUI waveTXT;
    public float waveTextDisplayTime = 3f;

    private void Start()
    {
        StartCoroutine(InitializeSpawner());
    }

    private IEnumerator InitializeSpawner()
    {
        // wait 1 frame so EnemyPool.Instance can initialize
        yield return null;

        spawnInterval = Mathf.Max(spawnInterval, limitDecrement);
        StartWave();
    }

    private void StartWave()
    {
        enemiesToSpawnInWave = startingEnemiesPerWave + enemyIncrementPerWave * (currentWave - 1);
        enemiesSpawnedThisWave = 0;

        StartCoroutine(ShowWaveText());
        StartCoroutine(SpawnWave());

        if (bossActivate && EnemyPool.Instance != null)
        {
            Transform bossSpawnPoint = spawner.Length > 1 ? spawner[1] : spawner[0];
            EnemyPool.Instance.SpawnFromPool("Boss", bossSpawnPoint.position, Quaternion.identity);
            bossActivate = false;
        }
    }

    private IEnumerator SpawnWave()
    {
        while (enemiesSpawnedThisWave < enemiesToSpawnInWave)
        {
            SpawnEnemy();
            enemiesSpawnedThisWave++;
            yield return new WaitForSeconds(spawnInterval);
        }

        currentWave++;
        spawnInterval = Mathf.Clamp(spawnInterval - spawnDecrement, limitDecrement, 999f);

        if (currentWave >= 6)
            bossActivate = true;

        yield return new WaitForSeconds(10f);
        StartWave();
    }

    private void SpawnEnemy()
    {

        EnemySpawnWave();
    }

    private IEnumerator ShowWaveText()
    {
        if (wavePanel != null)
        {
            wavePanel.SetActive(true);
            waveTXT.text = $"WAVE {currentWave}";
            yield return new WaitForSeconds(waveTextDisplayTime);
            wavePanel.SetActive(false);
        }
    }

    private void EnemySpawnWave()
    {
        Transform spawnPoint;

        // ----------------------------
        // ALWAYS RANDOM LEFT OR RIGHT
        // ----------------------------
        if (Random.value > 0.5f)
            spawnPoint = leftSpawner[Random.Range(0, leftSpawner.Length)];
        else
            spawnPoint = rightSpawner;

        // ----------------------------
        // WAVE 1 → ONLY enemyTags[0]
        // ----------------------------
        if (currentWave == 1)
        {
            SpawnEnemyAt(spawnPoint, enemyTags[0]);
            return;
        }

        // ----------------------------
        // WAVE 2+
        // ----------------------------
        int enemyIndex = Random.Range(0, enemyTags.Length);
        string enemyRanTag = enemyTags[enemyIndex];

        if (currentWave >= 5)
            SpawnEnemyAt(spawnPoint, enemyRanTag);
        else if (currentWave >= 4)
            SpawnEnemyAt(spawnPoint, enemyTags[3]);
        else if (currentWave >= 3)
            SpawnEnemyAt(spawnPoint, enemyTags[2]);
        else if (currentWave >= 2)
            SpawnEnemyAt(spawnPoint, enemyTags[1]);

    }

    private void SpawnEnemyAt(Transform spawnPoint, string enemyTag)
    {
        Vector3 dir;

        // RIGHT SPAWNER → MOVE LEFT
        if (spawnPoint == rightSpawner)
            dir = Vector3.left;
        else
            dir = Vector3.right;

        GameObject enemy = EnemyPool.Instance.SpawnFromPool(enemyTag, spawnPoint.position, spawnPoint.rotation);

        // Apply direction to ANY enemy script that has SetDirection()
        enemy.SendMessage("SetDirection", dir, SendMessageOptions.DontRequireReceiver);
    }

}
