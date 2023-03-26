using System;
using UnityEngine;

public class EnemyWallet : MonoBehaviour, IDropMoney
{
    [SerializeField] float _money;
    public float Money => _money;

    public event Action<MoneyDroppedInfo> OnDropMoney;

    public MoneyDroppedInfo MoneyInfo { get; private set; }

    private void Start() => MoneyInfo = new MoneyDroppedInfo(this.gameObject, Money);   

    public void DropMoney() => OnDropMoney?.Invoke(MoneyInfo);

    public void SetMoney(int amout) => _money = amout;
}


