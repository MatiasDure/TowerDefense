using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooler : MonoBehaviour
{
    [SerializeField] EnemiesToSpawn[] enemiesToAddInPool;

    private Dictionary<string, List<Enemy>> idEnemyPairs;

    public uint EnemiesActivated { get; private set; }

    public static EnemyPooler Instance { get; private set; } 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        idEnemyPairs = new();
        CreateEnemies();
    }

    private void CreateEnemies()
    {
        foreach(EnemiesToSpawn e in enemiesToAddInPool)
        {
            for(int i = 0; i < e.amountToSpawn; i++)
            {
                InstantiateEnemy(e.enemy);
            }
        }
    }

    private Enemy InstantiateEnemy (Enemy enemyToInstantiate)
    {
        Enemy newEnemy = Instantiate(enemyToInstantiate);
        newEnemy.gameObject.SetActive(false);

        string enemyId = newEnemy.Stats.ID;

        //Add enemy to the dictionary, matching with id
        if (!idEnemyPairs.ContainsKey(enemyId))
        {
            idEnemyPairs.Add(enemyId, new List<Enemy>());
            idEnemyPairs[enemyId].Add(newEnemy);
        }
        else idEnemyPairs[enemyId].Add(newEnemy);

        //add enemies to enemy manager
        EnemyManager.Instance.AddNewEnemy(newEnemy);

        return newEnemy;
    }

    private void ActivateEnemy(Enemy enemy)
    {
        enemy.gameObject.transform.position = transform.position;
        enemy.gameObject.SetActive(true);
        EnemiesActivated++;
    }
    
    public void EnableEnemyWithId(string id)
    {
        //going through the list of Enemy in the dictionary with the id passed
        foreach(Enemy enemy in idEnemyPairs[id])
        {
            if (enemy == null || enemy.gameObject.activeInHierarchy) continue;
            ActivateEnemy(enemy);
            return;
        }

        //in case there are no enemies available to activate, instantiate new one 
        Enemy newEnemy = idEnemyPairs[id][0];
        ActivateEnemy(InstantiateEnemy(newEnemy));
    }


    public void ResetEnemiesActivated() => EnemiesActivated = 0;
}
