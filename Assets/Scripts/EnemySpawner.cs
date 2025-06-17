using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyDatabaseSO enemyDatabase;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private float spawnInterval = 2f;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemyDatabase.enemyData.Count);
        EnemyData data = enemyDatabase.enemyData[index];

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject enemyGO = Instantiate(data.Prefab, spawnPoint.position, Quaternion.identity);

        EnemyBehavior behavior = enemyGO.GetComponent<EnemyBehavior>();
        if (behavior != null)
        {
            behavior.Initialize(data);
        }
    }
}
