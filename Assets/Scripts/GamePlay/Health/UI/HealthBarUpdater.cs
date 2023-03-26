using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(IHaveHealth))]
public class HealthBarUpdater : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private IHaveHealth healthProvider;

    private float maxValue;

    private void Awake()
    {
        if (!healthBar) Debug.LogWarning("Pass the health bar image to the HealthBarUpdater!");
        healthProvider = GetComponent<IHaveHealth>();
    }

    private void Start()
    {
        maxValue = healthProvider.StartingHealth;
        healthProvider.OnHealthChanged += UpdateHealthBar;
        healthProvider.OnHealthObjectDestroyed += Unsubscribe;
    }

    private void UpdateHealthBar(float changedValue)
    {
        if (!healthBar) return;
        healthBar.fillAmount = changedValue / maxValue;
    }

    private void Unsubscribe()
    {
        healthProvider.OnHealthChanged -= UpdateHealthBar;
        healthProvider.OnHealthObjectDestroyed -= Unsubscribe;
    }
}
