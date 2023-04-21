using UnityEngine;
using UnityEngine.UI;

///<summary>
/// This class updates the UI health bar image based on changes to the health of the object
/// that has the IHaveHealth component attached to it.
///</summary>
[RequireComponent(typeof(IHaveHealth))]
public class HealthBarUpdater : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private IHaveHealth _healthProvider;

    private float maxValue;

    private void Awake() => GetComponents();

    private void GetComponents()
    {
        _healthProvider = GetComponent<IHaveHealth>();
    }

    private void Start()
    {
        maxValue = _healthProvider.StartingHealth;
        _healthProvider.OnHealthChanged += UpdateHealthBar;
        _healthProvider.OnHealthObjectDestroyed += Unsubscribe;
    }

    /// <summary>
    /// Called by health events, this method updates the fill amount of the health bar image
    /// based on the current health of the object.
    /// </summary>
    /// <param name="changedValue"> The current health value of the object. </param>
    private void UpdateHealthBar(float changedValue)
    {
        if (!_healthBar) return;
        _healthBar.fillAmount = changedValue / maxValue;
    }

    /// <summary>
    /// Called when the object is destroyed, this method unsubscribes from the health events
    /// to prevent errors from being thrown after the object no longer exists.
    /// </summary>
    private void Unsubscribe()
    {
        _healthProvider.OnHealthChanged -= UpdateHealthBar;
        _healthProvider.OnHealthObjectDestroyed -= Unsubscribe;
    }
}
