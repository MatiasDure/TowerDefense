using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const string GAME_OVER = "Game Over\n";

    [SerializeField] Image gameOverScreen;
    [SerializeField] TextMeshProUGUI gameOverText;

    public static GameManager Instance { get; private set; }

    public bool IsGamePaused { get; private set; }
    public bool IsGameLost { get; private set; }
    public bool IsGameWon { get; private set; }

    private void Awake()
    {
        //Application.targetFrameRate = 20;
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    private void Start()
    {
        Castle.OnCastleDeath += GameLost;
        WaveManager.OnNoMoreWaves += GameWon;
    }

    private void GameLost()
    {
        IsGameLost = true;
        PauseGame();
        GameOver("You Lose");
    }

    private void GameWon()
    {
        IsGameWon = true;
        PauseGame();
        GameOver("You Win");
    }

    private void GameOver(string wonLost)
    {
        gameOverText.text = GAME_OVER + wonLost;
        gameOverScreen.gameObject.SetActive(true);
    }

    private void PauseGame()
    {
        IsGamePaused = true;
        Time.timeScale = 0;
    }

    private void OnDestroy()
    {
        Castle.OnCastleDeath -= GameLost;
        WaveManager.OnNoMoreWaves -= GameWon;
    }
}
