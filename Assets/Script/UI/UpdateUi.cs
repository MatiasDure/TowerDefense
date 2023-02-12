using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateUi : MonoBehaviour
{
    const uint INIT_LVL = 1;
    const float INIT_MONEY = 0; 

    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI waveText;

    private void Awake()
    {
        if (!moneyText) moneyText = GameObject.Find("TotalMoney").GetComponent<TextMeshProUGUI>();
        if (!waveText) waveText = GameObject.Find("WaveNumber").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Wallet.OnMoneyUpdated += UpdateMoneyText;
        WaveManager.OnWaveChange += UpdateWaveText;
        UpdateMoneyText(INIT_MONEY);
        UpdateWaveText(INIT_LVL);
    }

    private void UpdateMoneyText(float amount) => moneyText.text = "$"+Wallet.Instance.Money.ToString();

    private void UpdateWaveText(uint level) => waveText.text = "Wave: " + level.ToString();
}
