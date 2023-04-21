using UnityEngine;

/// <summary>
/// Struct that holds information about money dropped by an object.
/// </summary>
public struct MoneyDroppedInfo
{
    /// <summary>
    /// The amount of money that was dropped.
    /// </summary>
    public float Money { get; private set; }

    /// <summary>
    /// The object that dropped the money.
    /// </summary>
    public GameObject Owner { get; private set; }

    /// <summary>
    /// Creates a new instance of the MoneyDroppedInfo struct with the given parameters.
    /// </summary>
    /// <param name="objCalled">The object that dropped the money.</param>
    /// <param name="money">The amount of money that was dropped.</param>
    public MoneyDroppedInfo(GameObject objCalled, float money)
    {
        this.Money = money;
        this.Owner = objCalled;
    }
}





