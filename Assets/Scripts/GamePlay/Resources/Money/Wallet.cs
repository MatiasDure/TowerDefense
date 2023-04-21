using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Wallet class represents a wallet that can contain money. 
/// It is responsible for updating the player's money value.
/// </summary>
public class Wallet : Singleton<Wallet>
{
    [SerializeField] private float _money;

    /// <summary>
    /// The current amount of money in the wallet.
    /// </summary
    public float Money => _money;

    /// <summary>
    /// The event that is raised whenever the player's money value is updated.
    /// </summary>
    public static event Action<float> OnMoneyUpdated;

    protected override void Awake() => base.Awake();

    private void Start() => UpdateSubscription(true);

    /// <summary>
    /// Updates the subscription to the enemy's wallet OnDropMoney event
    /// </summary>
    /// <param name="subscribe"> A boolean passed to determine whether to subscribe or unsubscribe from the event </param>
    private void UpdateSubscription(bool subscribe)
    {
        List<Enemy> enemyList = EnemyManager.Instance.GetEnemiesInScene();
        foreach (Enemy e in enemyList)
        {
            if (subscribe) e.EnemyWallet.OnDropMoney += UpdateWallet;
            else e.EnemyWallet.OnDropMoney -= UpdateWallet;
        }
    }

    /// <summary>
    /// Updates the player's wallet based on the money dropped by an enemy.
    /// </summary>
    /// <param name="args">The MoneyDroppedInfo object containing the amount of money dropped and the enemy that dropped it.</param>
    private void UpdateWallet(MoneyDroppedInfo args) => UpdateWallet(args.Money);

    /// <summary>
    /// Updates the player's wallet based on a given amount of money.
    /// </summary>
    /// <param name="amount">The amount of money to add to the player's wallet.</param>

    public void UpdateWallet(float amount)
    {
        _money += amount;
        OnMoneyUpdated?.Invoke(amount);
    }
    
    private void OnDestroy() => UpdateSubscription(false);
}
