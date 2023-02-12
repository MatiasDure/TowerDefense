using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Wave", menuName = "ScriptableObjects/Wave")]
public class SO_Wave : ScriptableObject
{
    public EnemiesToSpawn[] enemies;
    public uint waveNumber = 0;
    public float delayBetweenSpawn = 0;
    public float delayBeforeNextWave = 0;
}

