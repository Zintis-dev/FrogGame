using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "ScriptableObjects/EnemyDatabase")]
public class EnemyDatabaseSO : ScriptableObject
{
    public List<EnemyData> enemyData;

    void Start()
    {

    }

    void Update()
    {

    }
}

[Serializable]
public class EnemyData
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int ID { get; private set; }

    [field: SerializeField]
    public GameObject Prefab { get; private set; }

    [field: SerializeField]
    public float Health { get; private set; }

    [field: SerializeField]
    public float Damage { get; private set; }

    [field: SerializeField]
    public float Speed { get; private set; }

    [field: SerializeField]
    public float AttackRange { get; private set; }

    [field: SerializeField]
    public float AttackCooldown { get; private set; }

    [field: SerializeField]
    public int Reward { get; private set; }

    [field: SerializeField]
    public Sprite Icon { get; private set; }

    [field: SerializeField]
    public string Description { get; private set; }
}
