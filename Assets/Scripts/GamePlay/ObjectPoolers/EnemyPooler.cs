using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a pool of enemies, enabling and disabling them as needed.
/// </summary>
/// <remarks>
/// The class keeps track of the enemies added to the pool, and allows for enabling and disabling them as needed.
/// </remarks>
public class EnemyPooler : Singleton<EnemyPooler>
{
    [SerializeField] private EnemiesToSpawn[] _enemiesToAddInPool;

    private Dictionary<string, List<Enemy>> _typeEnemy = new();

    /// <summary>
    /// The number of enemies that have been activated from the pool.
    /// </summary>
    public uint EnemiesActivated { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        CreateEnemies();
    }

    /// <summary>
    /// Creates the enemies specified in the <c>_enemiesToAddInPool</c> array.
    /// </summary>
    private void CreateEnemies()
    {
        foreach(EnemiesToSpawn e in _enemiesToAddInPool)
        {
            for(int i = 0; i < e.amountToSpawn; i++)
            {
                InstantiateEnemy(e.enemy);
            }
        }
    }

    /// <summary>
    /// Instantiates an Enemy object
    /// </summary>
    /// <param name="enemyToInstantiate">The Enemy prefab to instantiate.</param>
    /// <returns>The instantiated Enemy object.</returns>
    private Enemy InstantiateEnemy (Enemy enemyToInstantiate)
    {
        Enemy newEnemy = Instantiate(enemyToInstantiate);
        newEnemy.gameObject.SetActive(false);

        string enemyType = newEnemy.Stats.Type;

        AddEnemyToPool(newEnemy, enemyType);

        EnemyManager.Instance.AddNewEnemy(newEnemy);

        return newEnemy;
    }

    /// <summary>
    /// Adds an enemy to the pool of enemies
    /// </summary>
    /// <param name="newEnemy"> The enemy game object </param>
    /// <param name="enemyType"> A string representing the type of enemy to add to the pool </param>
    private void AddEnemyToPool(Enemy newEnemy, string enemyType)
    {
        if (!_typeEnemy.ContainsKey(enemyType))
        {
            _typeEnemy.Add(enemyType, new List<Enemy>());
            _typeEnemy[enemyType].Add(newEnemy);

            return;
        }
        
        _typeEnemy[enemyType].Add(newEnemy);
    }

    /// <summary>
    /// Activates an enemy game object
    /// </summary>
    /// <param name="enemy"> The Enemy script attached to the Enemy game object to actiavete </param>
    private void ActivateEnemy(Enemy enemy)
    {
        enemy.gameObject.transform.position = transform.position;
        enemy.gameObject.SetActive(true);
        EnemiesActivated++;
    }

    /// <summary>
    /// Enables an enemy with the specified type.
    /// </summary>
    /// <param name="type">The type of the enemy to activate.</param>
    /// <remarks> If there are no inactive enemies of that type available, a new one will be instantiated. </remarks>
    public void EnableEnemyByType(string type)
    {
        //going through the list of Enemy in the dictionary with the id passed
        foreach(Enemy enemy in _typeEnemy[type])
        {
            if (enemy == null || enemy.gameObject.activeInHierarchy) continue;
            ActivateEnemy(enemy);
            return;
        }

        //in case there are no enemies available to activate, instantiate new one 
        Enemy newEnemy = _typeEnemy[type][0];
        ActivateEnemy(InstantiateEnemy(newEnemy));
    }

    /// <summary>
    /// Resets the number of activated enemies to 0.
    /// </summary>
    public void ResetEnemiesActivated() => EnemiesActivated = 0;
}
