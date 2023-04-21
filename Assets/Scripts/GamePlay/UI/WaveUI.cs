using TMPro;
using UnityEngine;

/// <summary>
/// Displays the current wave timer on a TextMeshProUGUI UI element.
/// </summary>
public class WaveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerUI;

    private void Start() => SubscribeToEvents();
    
    private void OnDestroy() => WaveManager.OnTimerChanged -= UpdateTimerText;

    /// <summary>
    /// Subscribe to necessary events
    /// </summary>
    private void SubscribeToEvents() => WaveManager.OnTimerChanged += UpdateTimerText;

    /// <summary>
    /// Updates the timer display text with the provided string.
    /// </summary>
    /// <param name="text"> The string to display on the timer TextMeshProUGUI element. </param>
    private void UpdateTimerText(string text = "") => _timerUI.text = text;
}
