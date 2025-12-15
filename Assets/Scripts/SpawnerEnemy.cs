using System.Collections;
using UnityEngine;
using TMPro;

public class SpawnerEnemy : MonoBehaviour
{
    public static SpawnerEnemy Instance;

    [Header("----- Enemy Prefabs & Tags -----")]
    public string[] enemyTags;

    [Header("----- Spawner Points -----")]
    public Transform[] spawner;

    [Header("----- Spawn Settings -----")]
    public float spawnInterval = 3f;
    public float spawnDecrement = 0.2f;
    public float limitDecrement = 1f;

    [Header("----- Wave Settings -----")]
    public int startingEnemiesPerWave = 7;
    public int enemyIncrementPerWave = 3;
    public int maxWave = 2;

    private int currentWave = 1;
    private int enemiesToSpawnInWave;
    private int enemiesSpawnedThisWave;
    private int aliveEnemies;

    private bool waveSpawningFinished;

    [Header("----- Wave UI -----")]
    public GameObject wavePanel;
    public GameObject levelClearPanel;
    public TextMeshProUGUI waveTXT;
    public float waveTextDisplayTime = 3f;

    [Header("----- Panel for Insect UI -----")]
    public GameObject hopperPanel;
    public GameObject flyPanel;
    public GameObject bugPanel;
    public GameObject beePanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(InitializeSpawner());
        levelClearPanel.SetActive(false);
    }

    private IEnumerator InitializeSpawner()
    {
        yield return null;
        spawnInterval = Mathf.Max(spawnInterval, limitDecrement);
        StartWave();
    }

    private void StartWave()
    {
        waveSpawningFinished = false;

        enemiesToSpawnInWave = startingEnemiesPerWave + enemyIncrementPerWave * (currentWave - 1);
        enemiesSpawnedThisWave = 0;

        StartCoroutine(ShowWaveText());
        StartCoroutine(SpawnWave());
        StartCoroutine(InsectPanel());
    }

    private IEnumerator SpawnWave()
    {
        while (enemiesSpawnedThisWave < enemiesToSpawnInWave)
        {
            SpawnEnemy();
            enemiesSpawnedThisWave++;
            yield return new WaitForSeconds(spawnInterval);
        }

        waveSpawningFinished = true;
        spawnInterval = Mathf.Clamp(spawnInterval - spawnDecrement, limitDecrement, 999f);

        CheckWaveClear();
    }

    private void SpawnEnemy()
    {
        if (currentWave > maxWave) return;

        Transform spawnPoint = spawner[Random.Range(0, spawner.Length)];
        string enemyTag = enemyTags[0];

        if (currentWave >= 2) enemyTag = enemyTags[1];

        GameObject enemy = EnemyPool.Instance.SpawnFromPool(enemyTag, spawnPoint.position, Quaternion.identity);
    }

    public void RegisterEnemy()
    {
        aliveEnemies++;
    }

    public void UnregisterEnemy()
    {
        aliveEnemies = Mathf.Max(0, aliveEnemies - 1);
        CheckWaveClear();
    }

    private void CheckWaveClear()
    {
        if (!waveSpawningFinished || aliveEnemies > 0) return;

        if (currentWave >= maxWave)
        {
            LevelClear();
        }
        else
        {
            StartCoroutine(NextWaveDelay());
        }
    }

    private IEnumerator NextWaveDelay()
    {
        yield return new WaitForSeconds(3f);
        currentWave++;
        StartWave();
    }

    private void LevelClear()
    {
        StartCoroutine(LevelClearDelay());   
    }
    private IEnumerator LevelClearDelay()
    {
        levelClearPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f;
    }

    private IEnumerator ShowWaveText()
    {
        wavePanel.SetActive(true);
        waveTXT.text = $"WAVE {currentWave}";
        yield return new WaitForSeconds(waveTextDisplayTime);
        wavePanel.SetActive(false);
    }

    private IEnumerator InsectPanel()
    {
        yield return new WaitForSeconds(1.5f);

        if (currentWave == 1) hopperPanel.SetActive(true);
        if (currentWave == 2) flyPanel.SetActive(true);
        if (currentWave == 3) bugPanel.SetActive(true);
        if (currentWave == 4) beePanel.SetActive(true);

        if (currentWave <= 4) Time.timeScale = 0f;
    }

    public void InsectContinueOne()
    {
        hopperPanel.SetActive(false);
        flyPanel.SetActive(false);
        bugPanel.SetActive(false);
        beePanel.SetActive(false);

        Time.timeScale = 1f;
    }
}
