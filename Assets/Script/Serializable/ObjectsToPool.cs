using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectsToPool
{
    public ObjectPooler.PoolObjType objType;
    public GameObject prefab;
    public int amountToPool;

    [HideInInspector] public List<GameObject> gameObjectsInPool = new();
}