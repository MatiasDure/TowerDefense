using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveParser : MonoBehaviour
{
    [SerializeField] SO_Wave[] waves;

    //wave
    public int AmountWaves => waves.Length;
    public uint WaveNumber { get; private set; }
    public float DelayBeforeNextWave { get; private set; }
    public float DelayBetweenSpawn { get; private set; }

    //enemies
    public uint AmountEnemiesToSpawn { get; private set; }
    public EnemiesToSpawn[] EnemiesSpawn { get; private set; }

    public void Awake()
    {
        WaveNumber = AmountEnemiesToSpawn = 0;
        DelayBeforeNextWave = DelayBetweenSpawn = 0;
    }

    public void UpdateWaveValues(uint currentWaveIndex)
    {
        //assigning new values for next wave
        WaveNumber = waves[currentWaveIndex].waveNumber;
        EnemiesSpawn = waves[currentWaveIndex].enemies;

        foreach (EnemiesToSpawn enemies in EnemiesSpawn) AmountEnemiesToSpawn += (uint)enemies.amountToSpawn;

        DelayBetweenSpawn = waves[currentWaveIndex].delayBetweenSpawn;
        DelayBeforeNextWave = waves[currentWaveIndex].delayBeforeNextWave;
    }

    public void ResetEnemiesToSpawn() => AmountEnemiesToSpawn = 0;
    public void DecreaseDelayNextWave(float decreaseBy) => DelayBeforeNextWave -= decreaseBy;
}

