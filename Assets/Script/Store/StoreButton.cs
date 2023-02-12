using UnityEngine;
using System;

public class StoreButton : MonoBehaviour
{
    [SerializeField] ButtonElements _button;
    public bool CanClick { get; private set; }
    public ButtonElements Button => _button;

    private void Awake()
    {
        if(!_button.Img) throw new NullReferenceException("Image for store button not passed!");
    }

    private void Start()
    {
        Wallet.OnMoneyUpdated += UpdateButton;
        UpdateButton(0);
        SetButtonPrice(_button.Cost);
    }

    private void UpdateButton(float amount)
    {
        if(Wallet.Instance.Money < _button.Cost)
        {
            _button.Img.color  = Color.red;
            CanClick = false;
            return;
        }
        _button.Img.color = Color.green;
        CanClick = true;
    }

    private void SetButtonPrice(int price) => _button.TextObj.text = "" + price;
}


