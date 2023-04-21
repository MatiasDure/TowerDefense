using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describes an in-game enemy game object and its components
/// </summary>
[RequireComponent(typeof(WaypointFollower), typeof(EnemyWallet), typeof(EnemyHealth))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private SO_Enemy _enemyStats;
    [SerializeField] private EnemyWallet _enemyWallet;
    [SerializeField] private EnemyHealth _enemyHealth;

    private List<DebuffAttackController> _debuffedBy = new();

    /// <summary>
    /// Gets this enemy instance's wallet
    /// </summary>
    public EnemyWallet EnemyWallet => _enemyWallet;

    /// <summary>
    /// Gets this enemy instance's Waypoint follower component 
    /// </summary>
    public WaypointFollower Follower { get; private set; }

    /// <summary>
    /// Gets this enemy instance's information
    /// </summary>
    /// <remarks> Component is a scriptable object </remarks>
    public SO_Enemy Stats => _enemyStats;

    /// <summary>
    /// Event that is triggered when an enemy instance's health is <= 0 or if they have collided with the castle
    /// </summary>
    public static event Action OnAnEnemyDeath;

    /// <summary>
    /// Event that is triggered when an enemy instance has died
    /// </summary>
    public event Action OnEnemyDead;

    /// <summary>
    /// Event that is triggered when this instance is disabled
    /// </summary>
    public event Action<GameObject> OnNotTargetable;

    void Awake() => GetComponents();

    private void Start() => _enemyHealth.OnHealthZero += EnemyDied;

    private void OnEnable() => ResetEnemyStats();
    
    private void OnDisable() => OnNotTargetable?.Invoke(this.gameObject);

    private void GetComponents()
    {
        _enemyHealth = GetComponent<EnemyHealth>();
        Follower = GetComponent<WaypointFollower>();
    }

    /// <summary>
    /// Reacts accordingly after the enemy instance has been killed
    /// </summary>
    private void EnemyDied()
    {
        OnEnemyDead?.Invoke();
        DisableEnemy();
    }

    /// <summary>
    /// Resets enemy values as to what they were intially set as
    /// </summary>
    /// <remarks>Needed because of the use of object pooling</remarks>
    private void ResetEnemyStats()
    {
        _enemyHealth.ResetHealth();
        _debuffedBy.Clear();
    }

    /// <summary>
    /// Decreases the health points of the enemy health component
    /// </summary>
    /// <param name="damage">Amount of damage to inflict on the enemy health component</param>
    public void TakeDamage(float damage) => _enemyHealth.TakeDamage(damage);

    /// <summary>
    /// Modifies the speed on which the waypoint component is moving the enemy
    /// </summary>
    /// <param name="debuff">The debuff instance that triggered this effect to save it for later query</param>
    /// <param name="speedValue">The amount to set the speed to</param>
    public void UpdateSpeed(DebuffAttackController debuff, float speedValue)
    {
        Follower.Speed = speedValue;
        _debuffedBy.Add(debuff);
    }

    /// <summary>
    /// Checks whether a certain debuff instance has already debuffed this enemy
    /// </summary>
    /// <param name="debuffToCompare">The debuff instance we want to compare againts all the debuff instances that afected this enemy previously</param>
    /// <returns>True if the debuff instance passed as argument has debuffed this enemy, false otherwise</returns>
    public bool ComparePreviousDebuffers(DebuffAttackController debuffToCompare)
    {
        foreach(DebuffAttackController debuffer in _debuffedBy)
        {
            if (debuffer.Equals(debuffToCompare)) return true;
        }
        return false;
    }

    /// <summary>
    /// Disables this enemy instance
    /// </summary>
    public void DisableEnemy()
    {
        OnAnEnemyDeath?.Invoke();
        this.gameObject.SetActive(false);
    }
}
