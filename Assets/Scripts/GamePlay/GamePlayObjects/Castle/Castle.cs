using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describes an in-game castle game object, which is used to keep track of the player's life
/// </summary>
public class Castle : MonoBehaviour, IHaveHealth
{
    [SerializeField] private float _life;

    /// <summary>
    /// Gets the initial life assigned to the castle
    /// </summary>
    public float StartingHealth => _life; 

    /// <summary>
    /// Gets the current life of the castle
    /// </summary>
    public float Health { get; private set; }

    /// <summary>
    /// Event that is triggered when a castle instance's health is 0 or below
    /// </summary>
    public static event Action OnCastleDeath;

    /// <summary>
    /// Event that is triggered when an IHaveHealth object's health has been modified
    /// </summary>
    public event Action<float> OnHealthChanged;

    /// <summary>
    /// Event that is triggered when an IHaveHealth object has been destroyed
    /// </summary>
    public event Action OnHealthObjectDestroyed;

    private void Awake()
    {
        Health = StartingHealth;
    }

    /// <summary>
    /// Decreases the health points of a castle instance
    /// </summary>
    /// <param name="damage">Amount of damage to inflict on the castle</param>
    public void TakeDamage(float damage)
    {
        Health -= damage;
        OnHealthChanged?.Invoke(Health);
        if(Health <= 0) OnCastleDeath?.Invoke();
    }

    private void OnDestroy() => OnHealthObjectDestroyed?.Invoke();
}
