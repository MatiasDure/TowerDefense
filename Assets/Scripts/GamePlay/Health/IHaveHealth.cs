using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHaveHealth
{
    public float StartingHealth { get; }
    public float Health { get; }

    public event Action<float> OnHealthChanged;
    public event Action OnHealthObjectDestroyed;
    public void TakeDamage(float damage);
}
