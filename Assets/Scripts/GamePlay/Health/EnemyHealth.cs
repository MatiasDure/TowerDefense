using System;
using UnityEngine;

///<summary>
/// Class representing the health of an enemy.
///</summary>
public class EnemyHealth : MonoBehaviour, IHaveHealth
{
    [SerializeField] float _startingHealth;
    
    private bool _dead;

    /// <summary>
    /// Read-only property representing the starting health of the enemy.
    /// </summary>
    public float StartingHealth => _startingHealth;

    /// <summary>
    /// Property representing the current health of the enemy.
    /// </summary>
    public float Health { get; private set; }

    /// <summary>
    /// Event that is triggered when an IHaveHealth object's health has been modified
    /// </summary>
    public event Action<float> OnHealthChanged;

    /// <summary>
    /// Event that is triggered when an IHaveHealth object has been destroyed
    /// </summary>
    public event Action OnHealthObjectDestroyed;

    /// <summary>
    /// Event that is triggered when this enemy instance's hp has reached 0
    /// </summary>
    public event Action OnHealthZero;

    private void OnDestroy() => OnHealthObjectDestroyed?.Invoke();
    
    /// <summary>
    /// Checks whether the enemy has lost all its hp
    /// </summary>
    private void CheckIfDead()
    {
        if (Health <= 0) 
        {
            OnHealthZero?.Invoke(); 
            _dead = true; 
        }
    }

    ///<summary>
    /// Method to update the enemy's health.
    ///</summary>
    ///<param name="damage">The amount of damage taken.</param>
    public void TakeDamage(float damage)
    {
        if (_dead) return; 

        Health -= damage; 
        OnHealthChanged?.Invoke(Health); 

        CheckIfDead();
    }

    ///<summary>
    /// Method to reset the health of the enemy.
    ///</summary>
    public void ResetHealth()
    {
        OnHealthChanged?.Invoke(StartingHealth); 
        Health = StartingHealth; 
        _dead = false; 
    }
}
