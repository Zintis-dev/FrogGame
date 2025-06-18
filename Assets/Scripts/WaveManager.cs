using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private EnemyDatabaseSO enemyDatabase;
    [SerializeField] private List<EnemySpawner> spawners;
    [SerializeField] private float timeBetweenSpawns = 2f;
    [SerializeField] private float prepTime = 10f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Slider enemyProgressBar;

    private int currentWave = 1;
    private int enemiesToSpawn;
    private int enemiesKilled;
    private int spawnedCount;
    private bool isPreparingNextWave = false;

    private float targetSliderValue;

    private void Start()
    {
        UpdateWaveText();
        countdownText.gameObject.SetActive(false);
        enemyProgressBar.gameObject.SetActive(false);
        StartCoroutine(StartWave());
    }

    private IEnumerator StartWave()
    {
        Debug.Log($"--- Wave {currentWave} starting ---");

        countdownText.gameObject.SetActive(false);
        enemyProgressBar.gameObject.SetActive(true);

        enemiesKilled = 0;
        spawnedCount = 0;
        enemiesToSpawn = currentWave * 2;

        enemyProgressBar.maxValue = enemiesToSpawn;
        enemyProgressBar.value = enemiesToSpawn;
        targetSliderValue = enemiesToSpawn;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            EnemySpawner spawner = spawners[Random.Range(0, spawners.Count)];
            EnemyData data = enemyDatabase.enemyData[Random.Range(0, enemyDatabase.enemyData.Count)];
            spawner.SpawnEnemy(data, OnEnemyKilled);

            spawnedCount++;
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private void OnEnemyKilled()
    {
        enemiesKilled++;
        targetSliderValue = enemiesToSpawn - enemiesKilled;

        if (enemiesKilled >= spawnedCount && !isPreparingNextWave)
        {
            isPreparingNextWave = true;
            StartCoroutine(PrepBeforeNextWave());
        }
    }

    private IEnumerator PrepBeforeNextWave()
    {
        Debug.Log($"Wave {currentWave} completed. Preparing for next wave...");
        countdownText.gameObject.SetActive(true);
        enemyProgressBar.gameObject.SetActive(false);

        for (int i = (int)prepTime; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        currentWave++;
        UpdateWaveText();
        isPreparingNextWave = false;
        StartCoroutine(StartWave());
    }

    private void UpdateWaveText()
    {
        waveText.text = $"Wave: {currentWave}";
    }

    private void Update()
    {
        if (enemyProgressBar.gameObject.activeSelf)
        {
            enemyProgressBar.value = Mathf.Lerp(enemyProgressBar.value, targetSliderValue, Time.deltaTime * 8f);
        }
    }
}
