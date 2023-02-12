using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerUI;

    private void Awake() => WaveManager.OnTimerChanged += UpdateTimerText;

    private void UpdateTimerText(string text = "") => timerUI.text = text;

    private void OnDestroy() => WaveManager.OnTimerChanged -= UpdateTimerText;
}
