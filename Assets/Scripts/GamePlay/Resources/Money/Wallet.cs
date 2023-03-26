using System;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private float _money;

    public float Money => _money;
    public static Wallet Instance { get; private set; }
    
    public static event Action<float> OnMoneyUpdated;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    private void Start() => UpdateSubscription(true);

    private void UpdateSubscription(bool subscribe)
    {
        List<Enemy> enemyList = EnemyManager.Instance.GetEnemiesInScene();
        foreach (Enemy e in enemyList)
        {
            if (subscribe) e.EnemyWallet.OnDropMoney += UpdateWallet;
            else e.EnemyWallet.OnDropMoney -= UpdateWallet;
        }
    }

    public void UpdateWallet(MoneyDroppedInfo args) => UpdateWallet(args.Money);

    public void UpdateWallet(float amount)
    {
        _money += amount;
        OnMoneyUpdated?.Invoke(amount);
    }
    
    private void OnDestroy() => UpdateSubscription(false);

}
