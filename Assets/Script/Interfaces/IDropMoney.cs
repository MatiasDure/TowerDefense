using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IDropMoney
{
    public float Money { get; }
    public MoneyDroppedInfo MoneyInfo { get; }
    public event Action<MoneyDroppedInfo> OnDropMoney;

    public void DropMoney();
}
