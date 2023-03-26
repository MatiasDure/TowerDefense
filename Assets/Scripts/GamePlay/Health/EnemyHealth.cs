using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHaveHealth
{
    [SerializeField] float _health;

    private bool dead;
    public float StartingHealth => _health;

    public float Health { get; private set; }

    public event Action<float> OnHealthChanged;
    public event Action OnHealthObjectDestroyed;
    public event Action OnHealthZero;

    public void TakeDamage(float damage)
    {
        if (dead) return;

        Health -= damage;
        OnHealthChanged?.Invoke(Health);

        if (Health <= 0)
        {
            OnHealthZero?.Invoke();
            dead = true;
        }
    }

    public void ResetHealth()
    {
        OnHealthChanged?.Invoke(StartingHealth);
        Health = StartingHealth;
        dead = false;
    }

    private void OnDestroy() => OnHealthObjectDestroyed?.Invoke();

}
