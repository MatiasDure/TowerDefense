using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A struct that contains information about a specific object type to pool
/// </summary>
[System.Serializable]
public class ObjectPool
{
    public ObjectPooler.PoolObjType objType;
    public GameObject prefab;
    public int amountToPool;

    [HideInInspector] public List<GameObject> gameObjectsInPool = new();
}