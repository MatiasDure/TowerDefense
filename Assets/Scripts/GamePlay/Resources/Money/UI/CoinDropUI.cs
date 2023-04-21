using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Used to display a coin UI when a coin is dropped, 
/// displaying the amount of coins dropped for a short period of time.
/// </summary>
[RequireComponent(typeof(Canvas))]
public class CoinDropUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _secondsUntilDeactivate;
    [SerializeField] private string _animatorParameterName;

    private void Awake() => GetComponents();

    private void GetComponents()
    {
        if (!_canvas) _canvas = GetComponent<Canvas>();
        if (!_animator) _animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// Coroutine that waits for a specified amount of time before deactivating the game object.
    /// </summary>
    private IEnumerator TimeToDisapear()
    {
        yield return new WaitForSeconds(_secondsUntilDeactivate);
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the position and rotation of the canvas when displaying it
    /// </summary>
    /// <param name="body"> The transform of the body where the coin was dropped </param>
    private void SetPositionRotation(Transform body)
    {
        _canvas.transform.LookAt(Camera.main.transform);
        _canvas.transform.position = body.position;
    }

    /// <summary>
    /// Sets the text to display when canvas is active
    /// </summary>
    /// <param name="amountDropped"> Amount of coins dropped </param>
    private void SetAmountText(uint amountDropped) => _amountText.text = "+" + amountDropped;

    /// <summary>
    /// Displays the coin ui where the coins where dropped
    /// </summary>
    /// <param name="body"> The transform of the body where the coin was dropped </param>
    /// <param name="amountDropped"> Amount of coins dropped </param>
    public void AppearCoinUiAtBody(Transform body, uint amountDropped)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(TimeToDisapear());
        SetAmountText(amountDropped);
        SetPositionRotation(body);
        _animator.SetBool(_animatorParameterName, true);
    }
}
