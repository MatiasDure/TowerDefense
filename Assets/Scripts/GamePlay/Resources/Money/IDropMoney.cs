using System;

/// <summary>
/// Interface for objects that can drop money.
/// </summary>
public interface IDropMoney
{
    /// <summary>
    /// Gets information about the money that will be dropped.
    /// </summary>
    MoneyDroppedInfo MoneyInfo { get; }

    /// <summary>
    /// Event that is triggered when money is dropped.
    /// </summary>
    event Action<MoneyDroppedInfo> OnDropMoney;
}
