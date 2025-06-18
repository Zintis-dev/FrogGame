using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private EnemyDatabaseSO enemyDatabase;
    [SerializeField] private List<EnemySpawner> spawners;
    [SerializeField] private float timeBetweenSpawns = 2f;
    [SerializeField] private float prepTime = 10f;

    private int currentWave = 1;
    private int enemiesToSpawn;
    private int enemiesKilled;
    private int spawnedCount;
    private bool isPreparingNextWave = false;

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    private IEnumerator StartWave()
    {
        Debug.Log($"--- Wave {currentWave} starting ---");

        enemiesKilled = 0;
        spawnedCount = 0;
        enemiesToSpawn = currentWave * 2;

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

        if (enemiesKilled >= spawnedCount && !isPreparingNextWave)
        {
            isPreparingNextWave = true;
            StartCoroutine(PrepBeforeNextWave());
        }
    }

    private IEnumerator PrepBeforeNextWave()
    {
        Debug.Log($"Wave {currentWave} completed. Preparing for next wave...");

        for (int i = (int)prepTime; i > 0; i--)
        {
            Debug.Log($"Next wave starts in {i}...");
            yield return new WaitForSeconds(1f);
        }

        currentWave++;
        isPreparingNextWave = false;
        StartCoroutine(StartWave());
    }
}
