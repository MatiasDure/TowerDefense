using TMPro;
using UnityEngine;

/// <summary>
/// Updates the UI elements that display the current amount of money and wave number.
/// </summary>
public class UpdateUi : MonoBehaviour
{
    const uint INIT_LVL = 1;
    const float INIT_MONEY = 0; 

    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _waveText;

    private void Awake()
    {
        if (!_moneyText) _moneyText = GameObject.Find("TotalMoney").GetComponent<TextMeshProUGUI>();
        if (!_waveText) _waveText = GameObject.Find("WaveNumber").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        SubscriptToEvents();
        UpdateMoneyText(INIT_MONEY);
        UpdateWaveText(INIT_LVL);
    }

    /// <summary>
    /// Subscribe to necessary events
    /// </summary>
    private void SubscriptToEvents()
    {
        Wallet.OnMoneyUpdated += UpdateMoneyText;
        WaveManager.OnWaveChange += UpdateWaveText;
    }

    /// <summary>
    /// Updates the money text UI element to display the current amount of money.
    /// </summary>
    /// <param name="amount">The new amount of money.</param>
    private void UpdateMoneyText(float amount) => _moneyText.text = "$"+Wallet.Instance.Money.ToString();

    /// <summary>
    /// Updates the wave text UI element to display the current wave number.
    /// </summary>
    /// <param name="level">The new wave number.</param>
    private void UpdateWaveText(uint level) => _waveText.text = "Wave: " + level.ToString();
}
