using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Tower", menuName = "ScriptableObjects/Tower")]
public class SO_Tower : ScriptableObject
{
    [SerializeField] TowerManager _towerPrefab;
    [SerializeField] string _name;
    [SerializeField] float _cooldownShootTimer;
    [SerializeField] float _amountInflict;
    [SerializeField] float _range;
    [SerializeField] TowerManager _upgradedTower;

    public TowerManager TowerPrefab => _towerPrefab;
    public string Name => _name;
    public float Range => _range;
    public float CooldownShootTimer => _cooldownShootTimer;
    public float AmountInflict => _amountInflict;
    public TowerManager UpgradedTower => _upgradedTower;
}