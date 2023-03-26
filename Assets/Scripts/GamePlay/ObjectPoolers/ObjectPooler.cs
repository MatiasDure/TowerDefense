using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public enum PoolObjType
    {
        CoinUi,
    }

    [SerializeField] ObjectsToPool[] objectsToPool;

    public static ObjectPooler Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach(ObjectsToPool obj in objectsToPool)
        {
            for(int i = 0; i < obj.amountToPool; i++)
            {
                GameObject tmp = Instantiate(obj.prefab);
                tmp.SetActive(false);
                obj.gameObjectsInPool.Add(tmp);
            }
        }
    }

    public GameObject GetPooledObject(PoolObjType typeToGet)
    {
        foreach(ObjectsToPool obj in objectsToPool)
        {
            if(obj.objType == typeToGet)
            {
                foreach(GameObject pooledObj in obj.gameObjectsInPool)
                {
                    if(!pooledObj.activeInHierarchy) return pooledObj;
                }
                break;
            }
        }
        return null;
    }
}