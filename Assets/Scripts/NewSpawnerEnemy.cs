using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class Wave
{
    public string waveName;
    public string[] enemyTags;
    public int enemyCount;
    public float spawnRate;
}

public class NewSpawnerEnemy : MonoBehaviour
{
    public static NewSpawnerEnemy Instance;

    [Header("----- Waves -----")]
    public Wave[] waves;

    [Header("----- Spawner Points -----")]
    public Transform[] spawner;

    private int currentWaveIndex = 0;
    private int enemiesToSpawn;
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

    [Header("----- Fade Settings -----")]
    public CanvasGroup fadeCanvasGroup;
    public string pickingSceneName = "MapPicking";
    public float fadeDuration = 1f;

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
        StartWave();
    }

    private void StartWave()
    {
        waveSpawningFinished = false;

        // 🔥 RESET COUNTS (IMPORTANT)
        aliveEnemies = 0;
        enemiesSpawnedThisWave = 0;

        Wave wave = waves[currentWaveIndex];
        enemiesToSpawn = wave.enemyCount;

        Debug.Log("START WAVE: " + wave.waveName);

        StartCoroutine(ShowWaveText(wave.waveName));
        StartCoroutine(SpawnWave());
        StartCoroutine(InsectPanel());
    }

    private IEnumerator SpawnWave()
    {
        Wave wave = waves[currentWaveIndex];

        while (enemiesSpawnedThisWave < enemiesToSpawn)
        {
            SpawnEnemy();
            enemiesSpawnedThisWave++;
            yield return new WaitForSeconds(wave.spawnRate);
        }

        waveSpawningFinished = true;

        Debug.Log("Finished Spawning. Alive: " + aliveEnemies);

        CheckWaveClear();
    }

    private void SpawnEnemy()
    {
        Wave wave = waves[currentWaveIndex];

        Transform spawnPoint = spawner[Random.Range(0, spawner.Length)];
        string enemyTag = wave.enemyTags[Random.Range(0, wave.enemyTags.Length)];

        GameObject enemy = EnemyPool.Instance.SpawnFromPool(enemyTag, spawnPoint.position, Quaternion.identity);

        // ✅ ALWAYS REGISTER HERE
        RegisterEnemy();
    }

    public void RegisterEnemy()
    {
        aliveEnemies++;
        Debug.Log("Enemy Spawned. Alive: " + aliveEnemies);
    }

    public void UnregisterEnemy()
    {
        aliveEnemies = Mathf.Max(0, aliveEnemies - 1);
        Debug.Log("Enemy Removed. Alive: " + aliveEnemies);

        CheckWaveClear();
    }

    private void CheckWaveClear()
    {
        if (!waveSpawningFinished) return;
        if (aliveEnemies > 0) return;

        Debug.Log("WAVE CLEARED!");

        if (currentWaveIndex >= waves.Length - 1)
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
        currentWaveIndex++;
        StartWave();
    }

    private void LevelClear()
    {
        StartCoroutine(LevelClearSequence());
    }

    private IEnumerator LevelClearSequence()
    {
        levelClearPanel.SetActive(true);

        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(FadeToBlack());

        SceneManager.LoadScene(pickingSceneName);
    }

    private IEnumerator FadeToBlack()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;
    }

    private IEnumerator ShowWaveText(string waveName)
    {
        wavePanel.SetActive(true);
        waveTXT.text = waveName;

        yield return new WaitForSeconds(waveTextDisplayTime);
        wavePanel.SetActive(false);
    }

    private IEnumerator InsectPanel()
    {
        yield return new WaitForSeconds(1.5f);

        if (currentWaveIndex == 0) hopperPanel.SetActive(true);
        if (currentWaveIndex == 1) flyPanel.SetActive(true);
        if (currentWaveIndex == 2) bugPanel.SetActive(true);
        if (currentWaveIndex == 3) beePanel.SetActive(true);

        if (currentWaveIndex <= 2) Time.timeScale = 0f;
    }

    public void InsectContinueOne()
    {
        hopperPanel.SetActive(false);
        flyPanel.SetActive(false);
        bugPanel.SetActive(false);
        beePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public int GetCurrentWave()
    {
        return currentWaveIndex + 1;
    }
}