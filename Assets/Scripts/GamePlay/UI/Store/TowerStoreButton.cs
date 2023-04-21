using UnityEngine;
using System;

public class TowerStoreButton : MonoBehaviour
{
    /// <summary>
    /// The color of the button when the player can afford it.
    /// </summary>
    private readonly Color CAN_BUY = new Color(255, 255, 255, 1);

    /// <summary>
    /// The color of the button when the player cannot afford it.
    /// </summary>
    private readonly Color CANNOT_BUY = new Color(255, 255, 255, .3f);

    [SerializeField] private ButtonElements _button;

    /// <summary>
    /// Gets a true if the player has enough money to press on this button, false otherwise
    /// </summary>
    public bool CanClick { get; private set; }
    
    /// <summary>
    /// Gets a <c>ButtonElements</c> button which contains relevant information about a specific store button
    /// </summary>
    public ButtonElements Button => _button;

    private void Start()
    {
        SubscribeToEvents();
        UpdateButton();
        SetButtonPriceUiTxt(_button.Cost);
    }

    private void SubscribeToEvents()
    {
        Wallet.OnMoneyUpdated += UpdateButton;
    }

    /// <summary>
    /// Updates the button's color and clickability based on the player's available money.
    /// </summary>
    /// <param name="_"> Discarded argument </param>
    /// <remarks> 
    /// Parameter <c>_</c> is needed because the event the method is subscribed to requires it. Does not affect the code.
    /// </remarks>
    private void UpdateButton(float _ = 0)
    {
        if (Wallet.Instance.Money < _button.Cost)
        {
            SetButtonColor(CANNOT_BUY);
            CanClick = false;

            return;
        }

        SetButtonColor(CAN_BUY);
        CanClick = true;
    }

    /// <summary>
    /// Sets the color of the store button ui
    /// </summary>
    /// <param name="colorToSet"> The color to set the button to </param>
    private void SetButtonColor(Color colorToSet)
    {
        if (_button.Img == null) return;

        _button.Img.color = colorToSet;
    }

    /// <summary>
    /// Sets the button's price text.
    /// </summary>
    /// <param name="price">The price of the button.</param>
    private void SetButtonPriceUiTxt(int price) => _button.TextObj.text = "$" + price;
}


