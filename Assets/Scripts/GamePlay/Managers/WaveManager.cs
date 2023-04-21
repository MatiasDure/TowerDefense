using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the spawning of waves of enemies based on data from a WaveParser instance.
/// </summary>
[RequireComponent(typeof(WaveParser))]
public class WaveManager : MonoBehaviour
{
    /// <summary>
    /// The possible states of the WaveManager at every moment
    /// </summary>
    enum WaveState
    {
        SpawningEnemies,
        Waiting,
        UpdatingWave
    }

    private const string INCOMING_WAVE = "Next Wave In: ";
    private const float ONE_SECOND = 1;

    private WaveParser _waveParser;
    private uint _currentWaveIndex;
    private uint _enemiesKilled;
    private uint _currentEnemySpawnedIndex;
    private uint _indexOfEnemyToSpawn;
    private WaveState _currentState;
    private Coroutine _currentCoroutine;

    /// <summary>
    /// Event that is triggered when the current wave index changes.
    /// </summary>
    public static event Action<uint> OnWaveChange;

    /// <summary>
    /// Event that is triggered when there are no more waves to spawn.
    /// </summary>
    public static event Action OnNoMoreWaves;

    /// <summary>
    /// Event that is triggered when the timer for the next wave changes.
    /// </summary>
    public static event Action<string> OnTimerChanged;

    private void Awake()
    {
        _waveParser = GetComponent<WaveParser>(); 

        if (_waveParser.AmountWaves == 0) Debug.LogWarning("No waves were passed!");
    }

    void Start()
    {
        Enemy.OnAnEnemyDeath += UpdateEnemyKilled;
        _currentWaveIndex = 0;
        UpdateWaveValues();
    }

    /// <summary>
    /// Spawns enemies based on the data provided by the WaveParser.
    /// </summary>
    /// <returns>Coroutine that spawns enemies.</returns>
    private IEnumerator SpawnEnemies()
    {
        while (_currentState is WaveState.SpawningEnemies)
        {
            SetEnemyToSpawn();
            SpawnEnemy();

            if (ActivatedAllWaveEnemies())
            {
                UpdateWaveState(WaveState.Waiting);
                break;
            }

            yield return new WaitForSeconds(_waveParser.DelayBetweenSpawn);
        }

        _currentCoroutine = StartCoroutine(Waiting());
    }

    /// <summary>
    /// Waits for all enemies to be killed before updating the wave state to 'UpdatingWave' and starts the coroutine for updating the wave.
    /// </summary>
    /// <returns>An IEnumerator used for coroutines.</returns>
    private IEnumerator Waiting()
    {
        while (_currentState is WaveState.Waiting)
        {
            if (AllEnemiesDeactivated()) UpdateWaveState(WaveState.UpdatingWave);

            yield return null;
        }
        _currentCoroutine = StartCoroutine(UpdatingWave());
    }

    /// <summary>
    /// Updates the current wave with a delay before the next wave starts.
    /// </summary>
    private IEnumerator UpdatingWave()
    {
        _currentWaveIndex++;
        
        if (NoMoreWaves())
        {
            OnNoMoreWaves?.Invoke();
            StopAllCoroutines();
            yield return null;
        }

        while (_currentState is WaveState.UpdatingWave)
        {
            OnTimerChanged?.Invoke(INCOMING_WAVE + (int)_waveParser.DelayBeforeNextWave);

            if (_waveParser.DelayBeforeNextWave < ONE_SECOND)
            {
                yield return new WaitForSeconds(_waveParser.DelayBeforeNextWave);
                break;
            }
            yield return new WaitForSeconds(ONE_SECOND);

            _waveParser.DecreaseDelayNextWave(ONE_SECOND);
        }

        UpdateWaveValues();
    }

    /// <summary>
    /// Checks whether there are more waves available to parse
    /// </summary>
    /// <returns> True if there are still more waves, false otherwise </returns>
    private bool NoMoreWaves() => _currentWaveIndex >= _waveParser.AmountWaves;

    /// <summary>
    /// Checks whether all enemies spawned in the current wave have been deactivated 
    /// </summary>
    /// <returns> True if all enemies have been deactivated, false otherwise </returns>
    private bool AllEnemiesDeactivated() => _enemiesKilled == _waveParser.AmountEnemiesToSpawn;
    
    /// <summary>
    /// Determines whether all enemies for the current wave have been activated.
    /// </summary>
    /// <returns> Returns a bool indicating whether all enemies have been activated. </returns>
    private bool ActivatedAllWaveEnemies() => EnemyPooler.Instance.EnemiesActivated >= _waveParser.AmountEnemiesToSpawn;
    
    /// <summary>
    /// Updates the wave values
    /// </summary>
    private void UpdateWaveValues()
    {
        ResetValues();

        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);

        _waveParser.UpdateWaveValues(_currentWaveIndex);

        OnTimerChanged?.Invoke("");
        OnWaveChange?.Invoke(_waveParser.WaveNumber);

        _currentCoroutine = StartCoroutine(SpawnEnemies());
    }

    /// <summary>
    /// Spawns an enemy from the current wave's enemy list.
    /// </summary>
    private void SpawnEnemy()
    {
        if (_indexOfEnemyToSpawn < _waveParser.EnemiesSpawn.Length)
        {
            EnemyPooler.Instance.EnableEnemyByType(_waveParser.EnemiesSpawn[_indexOfEnemyToSpawn].enemy.Stats.Type);
            _currentEnemySpawnedIndex++;
        }
    }

    /// <summary>
    /// Sets the index of the enemy to spawn to the next index.
    /// </summary>
    private void SetEnemyToSpawn()
    {
        if (_currentEnemySpawnedIndex >= _waveParser.EnemiesSpawn[_indexOfEnemyToSpawn].amountToSpawn)
        {
            _currentEnemySpawnedIndex = 0;
            _indexOfEnemyToSpawn++;
        }
    }

    /// <summary>
    /// Updates the current state of the wave to the provided state.
    /// </summary>
    /// <param name="newState"> The new state to update to. </param>
    private void UpdateWaveState(WaveState newState) => _currentState = newState;

    private void ResetValues()
    {
        EnemyPooler.Instance.ResetEnemiesActivated();
        _waveParser.ResetEnemiesToSpawn();

        _currentState = WaveState.SpawningEnemies;
        _enemiesKilled = 0;
        _currentEnemySpawnedIndex = 0;
        _indexOfEnemyToSpawn = 0;
    }

    /// <summary>
    /// Increase the amount of enemies killed during each wave
    /// </summary>
    private void UpdateEnemyKilled() => _enemiesKilled++;

    private void OnDestroy() => Enemy.OnAnEnemyDeath -= UpdateEnemyKilled;
}

