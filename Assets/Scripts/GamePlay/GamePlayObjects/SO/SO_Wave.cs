using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Facilitates modifying a wave's gameplay
/// </summary>
[CreateAssetMenu(fileName = "SO_Wave", menuName = "ScriptableObjects/Wave")]
public class SO_Wave : ScriptableObject
{
    [SerializeField] public EnemiesToSpawn[] enemies;
    [SerializeField] public uint waveNumber = 0;
    [SerializeField] public float delayBetweenSpawn = 0;
    [SerializeField] public float delayBeforeNextWave = 0;
}

