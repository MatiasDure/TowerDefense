using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour, IHaveHealth
{
    [SerializeField] private float _life;

    public static event Action OnCastleDeath;

    public float StartingHealth { get => _life; }

    public float Health { get; private set; }

    public event Action<float> OnHealthChanged;
    public event Action OnHealthObjectDestroyed;

    private void Awake()
    {
        Health = StartingHealth;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        OnHealthChanged?.Invoke(Health);
        if(Health <= 0) OnCastleDeath?.Invoke();
    }

    private void OnDestroy() => OnHealthObjectDestroyed?.Invoke();
}
