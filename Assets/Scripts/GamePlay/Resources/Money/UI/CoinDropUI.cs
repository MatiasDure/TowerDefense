using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CoinDropUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Animator animator;
    [SerializeField] private float secondsUntilDeactivate;
    [SerializeField] private string animatorParameterName;

    private void Awake()
    {
        if(!canvas) canvas = GetComponent<Canvas>();
        if (!animator) animator = GetComponentInChildren<Animator>();
    }

    private IEnumerator TimeToDisapear()
    {
        yield return new WaitForSeconds(secondsUntilDeactivate);
        this.gameObject.SetActive(false);
    }

    public void AppearCoinUiAtBody(Transform body, uint amountDropped)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(TimeToDisapear());
        amountText.text = "+" + amountDropped;
        canvas.transform.LookAt(Camera.main.transform);
        canvas.transform.position = body.position;
        animator.SetBool(animatorParameterName, true);
    }

}
