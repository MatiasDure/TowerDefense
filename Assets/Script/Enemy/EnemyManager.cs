using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Dictionary<GameObject, Enemy> gameObjEnemies;
    private List<Enemy> enemies;

    public static EnemyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);

        gameObjEnemies = new();
        enemies = new();
    }

    public void AddNewEnemy(Enemy enemyScript)
    {
        gameObjEnemies.Add(enemyScript.gameObject, enemyScript);
        enemies.Add(enemyScript);
    }

    public Enemy FindEnemyInScene(GameObject enemyObj)
    {
        try
        {
            return gameObjEnemies[enemyObj];
        }
        catch
        {
            return null;
        }
    }

    public List<Enemy> GetEnemiesInScene() => enemies;
    
}
