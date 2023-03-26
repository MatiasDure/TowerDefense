using UnityEngine;
using System;

public class StoreButton : MonoBehaviour
{
    private readonly Color CAN_BUY = new Color(255, 255, 255, 1);
    private readonly Color CANNOT_BUY = new Color(255, 255, 255, .3f);

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
            _button.Img.color = CANNOT_BUY;
            CanClick = false;
            return;
        }
        _button.Img.color = CAN_BUY;
        CanClick = true;
    }

    private void SetButtonPrice(int price) => _button.TextObj.text = "$" + price;
}


