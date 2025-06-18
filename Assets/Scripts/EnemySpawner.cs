using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    public void SpawnEnemy(EnemyData data, Action onDeathCallback)
    {
        GameObject enemyGO = Instantiate(data.Prefab, spawnPoint.position, Quaternion.identity);
        enemyGO.tag = "Enemy";

        EnemyBehavior behavior = enemyGO.GetComponent<EnemyBehavior>();
        if (behavior != null)
        {
            behavior.Initialize(data);
            behavior.OnDeath += onDeathCallback;
        }
    }
}
