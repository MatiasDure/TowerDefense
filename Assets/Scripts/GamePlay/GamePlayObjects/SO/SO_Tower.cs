using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Facilitates modifying a tower's stats
/// </summary>
[CreateAssetMenu(fileName = "SO_Tower", menuName = "ScriptableObjects/Tower")]
public class SO_Tower : ScriptableObject
{
    [SerializeField] TowerManager _towerPrefab;
    [SerializeField] string _name;
    [SerializeField] float _cooldownShootTimer;
    [SerializeField] float _amountInflict;
    [SerializeField] float _range;
    [SerializeField] TowerManager _upgradedTower;

    /// <summary>
    /// Gets the prefab to instantiate from
    /// </summary>
    public TowerManager TowerPrefab => _towerPrefab;

    /// <summary>
    /// Gets a tower's name
    /// </summary>
    public string Name => _name;

    /// <summary>
    /// Gets the distance in which towers are able to "see" enemies
    /// </summary>
    public float Range => _range;

    /// <summary>
    /// Gets the cooldown duration a tower takes between shots
    /// </summary>
    public float CooldownShootTimer => _cooldownShootTimer;

    /// <summary>
    /// Gets the amount of damage a tower inflicts on enemies
    /// </summary>
    public float AmountInflict => _amountInflict;

    /// <summary>
    /// Gets the updgraded prefab version of each starting tower
    /// </summary>
    public TowerManager UpgradedTower => _upgradedTower;
}