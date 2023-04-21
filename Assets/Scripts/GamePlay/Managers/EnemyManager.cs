using System.Collections.Generic;
using UnityEngine;

///<summary>
/// This class keeps tracks of the enemies in the game
///</summary>
public class EnemyManager : Singleton<EnemyManager>
{
    private Dictionary<GameObject, Enemy> _gameObjEnemies = new();
    private List<Enemy> _enemies = new();

    protected override void Awake() => base.Awake();

    ///<summary>
    /// Adds a new enemy script and its gameobject to the manager to keep track of it.
    ///</summary>
    ///<param name="enemyScript"> The enemy script to add to the manager. </param>
    public void AddNewEnemy(Enemy enemyScript)
    {
        _gameObjEnemies.Add(enemyScript.gameObject, enemyScript);
        _enemies.Add(enemyScript);
    }

    ///<summary>
    /// Finds and returns the enemy scipt of a game object in the scene.
    ///</summary>
    ///<param name="enemyObj"> The game object associated with the enemy script to search for. </param>
    ///<returns> The enemy object associated with the provided game object, or null if it is not found. </returns>
    public Enemy FindEnemyInScene(GameObject enemyObj)
    {
        if (_gameObjEnemies.ContainsKey(enemyObj)) return _gameObjEnemies[enemyObj];

        return null;
    }

    ///<summary>
    /// Returns a list of all the enemies in the scene.
    ///</summary>
    public List<Enemy> GetEnemiesInScene() => _enemies;
    
}
