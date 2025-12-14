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

    [Header("----- Spawn Settings -----")]
    public float spawnInterval = 3f;     // initial time between spawns
    public float spawnDecrement = 0.2f;  // reduces spawn interval per wave
    public float limitDecrement = 1f;    // min allowed interval

    [Header("----- Wave Settings -----")]
    public int startingEnemiesPerWave = 7;
    public int enemyIncrementPerWave = 3;

    private int currentWave = 1;
    private int enemiesToSpawnInWave;
    private int enemiesSpawnedThisWave;

    [Header("----- Wave UI -----")]
    public GameObject wavePanel;
    public TextMeshProUGUI waveTXT;
    public float waveTextDisplayTime = 3f;

    [Header("----- Panel for Insect UI -----")]
    public GameObject hopperPanel;
    public GameObject flyPanel;
    public GameObject bugPanel;
    public GameObject beePanel;  

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
        StartCoroutine(InsectPanel());
    }

    private IEnumerator SpawnWave()
    {
        while (enemiesSpawnedThisWave < enemiesToSpawnInWave)
        {
            EnemySpawnWave();
            enemiesSpawnedThisWave++;
            yield return new WaitForSeconds(spawnInterval);
        }

        currentWave++;
        spawnInterval = Mathf.Clamp(spawnInterval - spawnDecrement, limitDecrement, 999f);

        yield return new WaitForSeconds(10f);
        StartWave();
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
        // --- Choose a random spawn point ---
        Transform spawnPoint = spawner[Random.Range(0, spawner.Length)];

        if (currentWave <= 6)
        {
            // --- Choose enemy tag for this wave ---
            string enemyTag = enemyTags[0]; // wave 1 default

            if (currentWave >= 2)
                enemyTag = enemyTags[1];
            if (currentWave >= 3)
                enemyTag = enemyTags[2];

            // Wave 5+ → random enemy
            if (currentWave >= 4)
                enemyTag = enemyTags[Random.Range(0, enemyTags.Length)];


            // --- Spawn enemy properly ---
            EnemyPool.Instance.SpawnFromPool(enemyTag, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("You win!");
        }
        
    }


    private IEnumerator InsectPanel()
    {
        if (currentWave == 1)
        {
            yield return new WaitForSeconds(1.5f);
            hopperPanel.SetActive(true);
            Time.timeScale = 0f;
            
        }
        if (currentWave == 2)
        {
            yield return new WaitForSeconds(1.5f);
            flyPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        if (currentWave == 3)
        {
            yield return new WaitForSeconds(1.5f);
            bugPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        if (currentWave == 4)
        {
            yield return new WaitForSeconds(1.5f);
            beePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void InsectContinueOne()
    {
        if (currentWave == 1)
        {
            hopperPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        if (currentWave == 2)
        {
            flyPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        if (currentWave == 3)
        {
            bugPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        if (currentWave == 4)
        {
            beePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

}
