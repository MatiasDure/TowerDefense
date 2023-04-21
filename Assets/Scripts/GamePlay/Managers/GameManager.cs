using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for managing the game state and ending the game in either a win or loss condition.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    private const string GAME_OVER = "Game Over\n";
    private const string WIN_TEXT = "You Win";
    private const string LOSE_TEXT = "You Lose";

    [SerializeField] private Image _gameOverScreen;
    [SerializeField] private TextMeshProUGUI _gameOverText;

    /// <summary>
    /// Indicates whether the game is currently paused
    /// </summary>
    public bool IsGamePaused { get; private set; }

    /// <summary>
    /// Indicates whether the game has been lost
    /// </summary>
    public bool IsGameLost { get; private set; }

    /// <summary>
    /// Indicates whether the game has been won
    /// </summary>
    public bool IsGameWon { get; private set; }

    protected override void Awake() => base.Awake();

    private void Start()
    {
        Castle.OnCastleDeath += GameLost;
        WaveManager.OnNoMoreWaves += GameWon;
    }

    /// <summary>
    /// Sets the losing screen
    /// </summary>
    private void GameLost()
    {
        IsGameLost = true;
        PauseGame();
        GameOver(LOSE_TEXT);
    }

    /// <summary>
    /// Sets the winning screen
    /// </summary>
    private void GameWon()
    {
        IsGameWon = true;
        PauseGame();
        GameOver(WIN_TEXT);
    }

    /// <summary>
    /// Displays the game over screen with a specified message.
    /// </summary>
    /// <param name="wonLost"> A string which states whether the game was won or lost </param>
    private void GameOver(string wonLost)
    {
        _gameOverText.text = GAME_OVER + wonLost;
        _gameOverScreen.gameObject.SetActive(true);
    }

    /// <summary>
    /// Pauses the game
    /// </summary>
    /// <remarks> Sets <c>Time.timeScale = 0</c> </remarks>
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
