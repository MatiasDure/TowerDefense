using System;
using UnityEngine;

/// <summary>
/// A class that manages the dropping of money when an enemy is killed.
/// </summary>
public class EnemyWallet : MonoBehaviour, IDropMoney
{
    [SerializeField] private float _money; 

    private Enemy _enemy;

    /// <summary>
    /// Event that is triggered when the enemy is killed and money is dropped.
    /// </summary>
    public event Action<MoneyDroppedInfo> OnDropMoney;

    /// <summary>
    /// The information about the dropped money.
    /// </summary>
    public MoneyDroppedInfo MoneyInfo { get; private set; }

    private void Awake() => GetComponents();

    private void Start()
    {
        _enemy.OnEnemyDead += DropMoney;
        MoneyInfo = new MoneyDroppedInfo(this.gameObject, _money);
    }

    private void OnDestroy() => _enemy.OnEnemyDead -= DropMoney;

    /// <summary>
    /// Gets the required components.
    /// </summary>
    private void GetComponents() => _enemy = GetComponent<Enemy>();

    /// <summary>
    /// Drops the money and invokes the OnDropMoney event.
    /// </summary>
    private void DropMoney() => OnDropMoney?.Invoke(MoneyInfo);
}



