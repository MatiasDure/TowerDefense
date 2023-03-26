using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaypointFollower), typeof(EnemyWallet), typeof(EnemyHealth))]
public class Enemy : MonoBehaviour
{
    [SerializeField] SO_Enemy _enemyStats;
    [SerializeField] EnemyWallet _enemyWallet;
    [SerializeField] EnemyHealth enemyHealth;

    public static event Action OnDeath;
    public event Action<GameObject> OnDisabled;

    public EnemyWallet EnemyWallet => _enemyWallet;
    public List<DebuffAttackController> DebuffedBy { get; private set; }
    public WaypointFollower Follower { get; private set; }
    public SO_Enemy Stats => _enemyStats;

    void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        Follower = GetComponent<WaypointFollower>();
        DebuffedBy = new();
    }

    private void Start() => enemyHealth.OnHealthZero += EnemyDied;

    private void EnemyDied()
    {
        _enemyWallet.DropMoney();
        DisableEnemy();
    }

    private void ResetEnemyStats()
    {
        enemyHealth.ResetHealth();
        DebuffedBy.Clear();
    }
    public void TakeDamage(float damage) => enemyHealth.TakeDamage(damage);
    public void UpdateSpeed(DebuffAttackController obj, float value)
    {
        Follower.SetSpeed(value);
        DebuffedBy.Add(obj);
    }

    public bool ComparePreviousDebuffers(DebuffAttackController objToCompare)
    {
        foreach(DebuffAttackController debuffer in DebuffedBy)
        {
            if (debuffer.Equals(objToCompare)) return true;
        }
        return false;
    }

    public void DisableEnemy()
    {
        OnDeath?.Invoke();
        this.gameObject.SetActive(false);
    }
    
    private void OnEnable() => ResetEnemyStats();
    
    private void OnDisable() => OnDisabled?.Invoke(this.gameObject);

}
