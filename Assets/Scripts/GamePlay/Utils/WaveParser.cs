using UnityEngine;

public class WaveParser : MonoBehaviour
{
    [SerializeField] private SO_Wave[] _waves;

    /// <summary>
    /// The number of waves defined in the game.
    /// </summary>
    public int AmountWaves => _waves.Length;

    /// <summary>
    /// The current wave number.
    /// </summary>
    public uint WaveNumber { get; private set; }

    /// <summary>
    /// The delay before the next wave starts.
    /// </summary>
    public float DelayBeforeNextWave { get; private set; }

    /// <summary>
    /// The delay between the spawn of enemies in the same wave.
    /// </summary>
    public float DelayBetweenSpawn { get; private set; }

    /// <summary>
    /// The total amount of enemies that will spawn in the current wave.
    /// </summary>
    public uint AmountEnemiesToSpawn { get; private set; }

    /// <summary>
    /// An array of enemies to be spawned in the current wave.
    /// </summary>
    public EnemiesToSpawn[] EnemiesSpawn { get; private set; }


    /// <summary>
    /// Updates the values of the current wave with the values of the next wave.
    /// </summary>
    /// <param name="currentWaveIndex"> The index of the next wave. </param>
    public void UpdateWaveValues(uint currentWaveIndex)
    {
        WaveNumber = _waves[currentWaveIndex].waveNumber;
        EnemiesSpawn = _waves[currentWaveIndex].enemies;

        foreach (EnemiesToSpawn enemies in EnemiesSpawn) AmountEnemiesToSpawn += (uint)enemies.amountToSpawn;

        DelayBetweenSpawn = _waves[currentWaveIndex].delayBetweenSpawn;
        DelayBeforeNextWave = _waves[currentWaveIndex].delayBeforeNextWave;
    }

    /// <summary>
    /// Resets the total amount of enemies that will spawn in the current wave to 0.
    /// </summary>
    public void ResetEnemiesToSpawn() => AmountEnemiesToSpawn = 0;

    /// <summary>
    /// Decreases the delay before the next wave by a certain amount.
    /// </summary>
    /// <param name="decreaseBy"> The amount to decrease the delay by. </param>
    public void DecreaseDelayNextWave(float decreaseBy) => DelayBeforeNextWave -= decreaseBy;
}

