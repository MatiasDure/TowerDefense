using UnityEngine;

/// <summary>
/// A class that manages the pooling of different game objects.
/// </summary>
public class ObjectPooler : Singleton<ObjectPooler>
{
    /// <summary>
    /// An enum that defines the types of game objects that can be pooled.
    /// </summary>
    public enum PoolObjType
    {
        CoinUi,
    }

    [SerializeField] private ObjectPool[] _pools;

    protected override void Awake() => base.Awake();

    private void Start() => InstantiatGameObjs();

    /// <summary>
    /// Instantiates the game objects to pool and adds them to their respective object pools.
    /// </summary>
    private void InstantiatGameObjs()
    {
        foreach (ObjectPool pool in _pools)
        {
            for (int i = 0; i < pool.amountToPool; i++)
            {
                GameObject tmp = Instantiate(pool.prefab);
                tmp.SetActive(false);
                pool.gameObjectsInPool.Add(tmp);
            }
        }
    }

    /// <summary>
    /// Finds an inactive game object from the specified object pool.
    /// </summary>
    /// <param name="poolToLookIn"> The object pool to search for inactive objects. </param>
    /// <returns> An inactive game object from the object pool, or null if none are found. </returns>
    private GameObject FindInactiveGameObj(ObjectPool poolToLookIn)
    {
        foreach (GameObject pooledObj in poolToLookIn.gameObjectsInPool)
        {
            if (!pooledObj.activeInHierarchy) return pooledObj;
        }

        return null;
    }

    /// <summary>
    /// Gets an inactive game object of the specified type from the object pool.
    /// </summary>
    /// <param name="typeToGet"> The type of game object to get from the object pool. </param>
    /// <returns> An inactive game object of the specified type, or null if none are found. </returns>
    public GameObject GetPooledObject(PoolObjType typeToGet)
    {
        foreach(ObjectPool pool in _pools)
        {
            if (pool.objType != typeToGet) continue;

            return FindInactiveGameObj(pool);
        }
        return null;
    }
}