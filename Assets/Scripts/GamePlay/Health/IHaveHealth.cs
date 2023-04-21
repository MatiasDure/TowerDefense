using System;

///<summary>
/// Interface for objects that have health.
///</summary>
public interface IHaveHealth
{
    ///<summary>
    /// The starting health of the object.
    ///</summary>
    public float StartingHealth { get; }

    ///<summary>
    /// The current health of the object.
    ///</summary>
    public float Health { get; }

    ///<summary>
    /// Event invoked when the health of the object changes.
    ///</summary>
    public event Action<float> OnHealthChanged;

    ///<summary>
    /// Event invoked when the object is destroyed.
    ///</summary>
    public event Action OnHealthObjectDestroyed;

    ///<summary>
    /// Method that takes damage as a parameter and subtracts it from the object's health.
    ///</summary>
    public void TakeDamage(float damage);
}
