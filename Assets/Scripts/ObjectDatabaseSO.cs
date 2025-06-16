using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu]

public class ObjectDatabaseSO : ScriptableObject
{
    public List<ObjectData> objectData;

    void Start()
    {

    }

    void Update()
    {

    }
}

[Serializable]
public class ObjectData
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int ID { get; private set; }

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;

    [field: SerializeField]
    public GameObject Prefab { get; private set; }

    [field: SerializeField]
    public int Damage { get; private set; }

    [field: SerializeField]
    public String Description { get; private set; }

    [field: SerializeField]
    public int Price { get; private set; }

    [field: SerializeField]
    public Sprite Icon { get; private set; }


}
