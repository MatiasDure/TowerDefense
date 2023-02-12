using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(WaveParser))]
public class WaveManager : MonoBehaviour
{
    const string INCOMING_WAVE = "Next Wave In: ";
    const float ONE_SECOND = 1;
    
    [SerializeField] WaveParser waveParser;

    //wave values
    private uint currentWaveIndex;

    //enemy values
    private uint enemiesKilled;
    private uint currentEnemySpawnedIndex;
    private uint indexOfEnemyToSpawn;

    Coroutine currentCoroutine;

    public static event Action<uint> OnWaveChange;
    public static event Action OnNoMoreWaves;
    public static event Action<string> OnTimerChanged;

    enum WaveState
    {
        SpawningEnemies,
        Waiting,
        UpdatingWave
    }

    private WaveState currentState;

    private void Awake()
    {
        //if (!enemySpawner) enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemyPooler>();
        if (waveParser.AmountWaves == 0) Debug.LogWarning("No waves were passed!");
    }

    // Start is called before the first frame update
    void Start()
    {
        Enemy.OnDeath += UpdateEnemyKilled;
        currentWaveIndex = 0;
        UpdateWaveValues();
    }

    private IEnumerator SpawnEnemies()
    {

        while (currentState is WaveState.SpawningEnemies)
        {
            //checking which enemy type to spawn
            if (currentEnemySpawnedIndex >= waveParser.EnemiesSpawn[indexOfEnemyToSpawn].amountToSpawn)
            {
                currentEnemySpawnedIndex = 0;
                indexOfEnemyToSpawn++;
            }

            //spawning enemies
            if (indexOfEnemyToSpawn < waveParser.EnemiesSpawn.Length)
            {  
                EnemyPooler.Instance.EnableEnemyWithId(waveParser.EnemiesSpawn[indexOfEnemyToSpawn].enemy.Stats.ID);
                currentEnemySpawnedIndex++;
            }

            //break case
            if (EnemyPooler.Instance.EnemiesActivated >= waveParser.AmountEnemiesToSpawn)
            {
                currentState = WaveState.Waiting;
                break;
            }

            yield return new WaitForSeconds(waveParser.DelayBetweenSpawn);
        }

        currentCoroutine = StartCoroutine(Waiting());
    }

    private IEnumerator Waiting()
    {
        //waiting for enemies to be killed
        while (currentState is WaveState.Waiting)
        {
            if (enemiesKilled == waveParser.AmountEnemiesToSpawn) currentState = WaveState.UpdatingWave;
            yield return null;
        }
        currentCoroutine = StartCoroutine(UpdatingWave());
    }

    private IEnumerator UpdatingWave()
    {
        currentWaveIndex++;
        if (currentWaveIndex >= waveParser.AmountWaves)
        {
            OnNoMoreWaves?.Invoke();
            StopAllCoroutines();
            yield return null;
        }

        //updating current wave
        while (currentState is WaveState.UpdatingWave)
        {
            OnTimerChanged?.Invoke(INCOMING_WAVE + (int)waveParser.DelayBeforeNextWave);

            if (waveParser.DelayBeforeNextWave < ONE_SECOND)
            {
                yield return new WaitForSeconds(waveParser.DelayBeforeNextWave);
                break;
            }
            yield return new WaitForSeconds(ONE_SECOND);

            waveParser.DecreaseDelayNextWave(ONE_SECOND);
        }

        UpdateWaveValues();
    }

    private void UpdateWaveValues()
    {
        ResetValues();

        if (currentCoroutine != null) StopCoroutine(currentCoroutine);

        waveParser.UpdateWaveValues(currentWaveIndex);

        OnTimerChanged?.Invoke("");
        OnWaveChange?.Invoke(waveParser.WaveNumber);

        currentCoroutine = StartCoroutine(SpawnEnemies());
    }

    private void ResetValues()
    {
        EnemyPooler.Instance.ResetEnemiesActivated();
        waveParser.ResetEnemiesToSpawn();

        currentState = WaveState.SpawningEnemies;
        enemiesKilled = 0;
        currentEnemySpawnedIndex = 0;
        indexOfEnemyToSpawn = 0;
    }

    private void UpdateEnemyKilled() => enemiesKilled++;

    private void OnDestroy() => Enemy.OnDeath -= UpdateEnemyKilled;
}

