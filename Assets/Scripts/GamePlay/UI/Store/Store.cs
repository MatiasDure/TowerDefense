using System;
using UnityEngine;

/// <summary>
/// Represents a store that allows the player to purchase and update towers.
/// </summary>
public class Store : MonoBehaviour
{
    private const string BTN_UPDATE = "Update";

    [SerializeField] private TowerStoreButton[] _allStoreButtons;
    [SerializeField] private GameObject[] _towerSelectedOptions;
    [SerializeField] private GameObject[] _purchaseButtons;

    private TowerManager _towerSelected;
    private TowerManager _towerBought;
    private bool _toUpdate;
    private bool _currentlyHoldingTower;
    private int _towerCost;

    /// <summary>
    /// An event that is triggered when the player purchases a tower from the store.
    /// </summary>
    public static event Action OnPurchasedTower;

    private void Start()
    {
        SubscribeToEvents();
    }
    private void Update() => StoreState();

    private void OnDestroy()
    {
        TowerSelector.OnTowerWantsUpdate -= SwitchToUpdate;
        TowerSelector.OnNoUpdateNeeded -= SwitchToBuy;
    }

    /// <summary>
    /// Subscribe to events
    /// </summary>
    private void SubscribeToEvents()
    {
        TowerSelector.OnTowerWantsUpdate += SwitchToUpdate;
        TowerSelector.OnNoUpdateNeeded += SwitchToBuy;
    }

    /// <summary>
    /// Determines the state of the store which are to update or buy a tower.
    /// </summary>
    private void StoreState()
    {
        if (_towerSelectedOptions.Length == 0 || 
            _purchaseButtons.Length == 0) return;
      
        if (_toUpdate && 
            !_towerSelectedOptions[0].activeInHierarchy) ToggleButtonsState(true);
        else if (!_toUpdate && 
            !_purchaseButtons[0].activeInHierarchy) ToggleButtonsState(false);
    }

    /// <summary>
    /// Toggles the purchase and update/remove buttons as necessary.
    /// </summary>
    /// <param name="activateUpdateBtn"> Whether the update/remove buttons should be active. </param>
    private void ToggleButtonsState(bool activateUpdateBtn)
    {
        foreach (GameObject gameObj in _purchaseButtons)
        {
            gameObj.SetActive(!activateUpdateBtn);
        }

        foreach (GameObject gameObj in _towerSelectedOptions)
        {
            gameObj.SetActive(activateUpdateBtn);
        }
    }

    /// <summary>
    /// Switches to update mode
    /// </summary>
    /// <param name="tower"> The tower that was selected by the player. </param>
    /// <remarks> Disables the first selected tower if two towers are selected. </remarks>
    private void SwitchToUpdate(TowerSelector tower)
    {
        if (_towerSelected != null && 
            _towerSelected.gameObject != tower.gameObject) _towerSelected.TowerSelector.Selected();

        _toUpdate = true;
        _towerSelected = tower.gameObject.GetComponent<TowerManager>();
    }

    /// <summary>
    /// Switches to buy mode.
    /// </summary>
    private void SwitchToBuy()
    {
        _towerSelected = null;
        _toUpdate = false;
    }

    /// <summary>
    /// Resets the store after a purchased tower was dropped
    /// </summary>
    /// <param name="isSuccess"> Whether the tower was placed successfully. </param>
    private void DropTower(bool isSuccess)
    {
        RefundMoney(!isSuccess);

        _towerBought.OnDropTower -= DropTower;
        _currentlyHoldingTower = false;
        _towerBought = null;
    }

    /// <summary>
    /// Refunds the player's money if the tower was not placed successfully.
    /// </summary>
    /// <param name="provideRefund"> Whether the store needs to provide a refund. </param>
    private void RefundMoney(bool provideRefund)
    {
        if (provideRefund) Wallet.Instance.UpdateWallet(_towerCost);
    }

    /// <summary>
    /// Disables the hover description of a button when toggling update/purchase buttons
    /// </summary>
    /// <param name="updateBtn"> The button to disable the description for </param>
    private static void DisableBtnDescription(TowerStoreButton updateBtn)
    {
        if (updateBtn.gameObject.TryGetComponent<ToggleObjUI>(out var toggler)) toggler.ManuallyDisableElement();
    }

    /// <summary>
    /// Descreases the player's money by the price established 
    /// </summary>
    /// <param name="amountToCharge"> The amount to decrease by </param>
    private static void ChargePlayer(float amountToCharge) => Wallet.Instance.UpdateWallet(-amountToCharge);

    /// <summary>
    /// Finds a button in the <c>_allStoreButtons</c> array by name
    /// </summary>
    /// <param name="btnName"> The name of the button to look for </param>
    /// <returns> The button that matches with the name passed, or null if no matches found </returns>
    private TowerStoreButton FindButtonByName(string btnName)
    {
        foreach (TowerStoreButton b in _allStoreButtons)
        {
            if (b.Button.Name != btnName) continue;

            return b;
        }

        return null;
    }

    /// <summary>
    /// Creates an instance of the tower purchased
    /// </summary>
    /// <param name="infoTowerBought"> Information about the tower purchased </param>
    private void CreatePurchasedTower(SO_Tower infoTowerBought)
    {
        _towerBought = Instantiate(infoTowerBought.TowerPrefab);
        _towerBought.OnDropTower += DropTower;
        _towerBought.SetBought();
    }

    /// <summary>
    /// Method called by UI upgrade button to upgrade tower.
    /// </summary>
    public void UpdateTower()
    {
        if (GameManager.Instance.IsGamePaused ||
            !_towerSelected ||
            !_towerSelected.HasUpgrade) return;

        TowerStoreButton updateBtn = FindButtonByName(BTN_UPDATE);

        if (!updateBtn.CanClick) return;

        ChargePlayer(updateBtn.Button.Cost);
        _towerSelected.UpdateTower();
        DisableBtnDescription(updateBtn);
        SwitchToBuy();
    }

    /// <summary>
    /// Method called by the UI Remove button to remove a tower game object.
    /// </summary>
    public void RemoveTower()
    {
        if (GameManager.Instance.IsGamePaused) return;

        _towerSelected.Placer.TowerSpot.TowerLeft();
        Destroy(_towerSelected.gameObject);

        SwitchToBuy();
    }

    /// <summary>
    /// Method called by a UI tower button to purchase a specific tower.
    /// </summary>
    /// <param name="toBuy"> The <c>SO_Tower</c> object representing the tower to purchase. </param>
    public void PurchaseTowerType(SO_Tower toBuy)
    {
        if (GameManager.Instance.IsGamePaused ||
            _currentlyHoldingTower ||
            string.IsNullOrEmpty(toBuy.Name)) return;

        TowerStoreButton towerToPurchase = FindButtonByName(toBuy.Name);

        if (!towerToPurchase.CanClick) return;

        ChargePlayer(towerToPurchase.Button.Cost);
        _towerCost = towerToPurchase.Button.Cost;
        CreatePurchasedTower(toBuy);
        _currentlyHoldingTower = true;
        OnPurchasedTower?.Invoke();
    }
}