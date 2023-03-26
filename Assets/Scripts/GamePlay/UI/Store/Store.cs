using System;
using UnityEngine;

public class Store : MonoBehaviour
{
    [SerializeField] StoreButton[] allStoreButtons;
    [SerializeField] GameObject[] towerSelectedOptions;
    [SerializeField] GameObject[] purchaseButtons;

    private bool toUpdate;
    private bool currentlyHoldingTower;
    private TowerManager towerSelected;
    private TowerManager towerBought;
    private int towerCost;

    public static event Action OnPurchasedTower;

    private void Start()
    {
        toUpdate = false;
        currentlyHoldingTower = false;
        TowerSelector.OnTowerWantsUpdate += SwitchToUpdate;
        TowerSelector.OnNoUpdateNeeded += SwitchToBuy;
    }

    private void Update() => StoreState();

    private void StoreState()
    {
        if (towerSelectedOptions.Length == 0 || 
            purchaseButtons.Length == 0) return;

        //Toggling between purchase buttons and update/remove buttons        
        if (toUpdate && 
            !towerSelectedOptions[0].activeInHierarchy) ToggleButtonsState(true);

        else if (!toUpdate && 
            !purchaseButtons[0].activeInHierarchy) ToggleButtonsState(false);
    }

    private void ToggleButtonsState(bool updateTowerButtonOn)
    {
        foreach (GameObject gameObj in purchaseButtons)
        {
            gameObj.SetActive(!updateTowerButtonOn);
        }

        foreach (GameObject gameObj in towerSelectedOptions)
        {
            gameObj.SetActive(updateTowerButtonOn);
        }
    }

    private void SwitchToUpdate(TowerSelector tower)
    {
        //if two towers are selected, disable the first selected tower
        if (towerSelected != null && 
            towerSelected.gameObject != tower.gameObject) towerSelected.TowerSelector.Selected();

        toUpdate = true;
        towerSelected = tower.gameObject.GetComponent<TowerManager>();
    }

    private void SwitchToBuy()
    {
        towerSelected = null;
        toUpdate = false;
    }

    private void DropTower(bool isSuccess)
    {
        //refund money if tower was not placed in spot
        if(!isSuccess) Wallet.Instance.UpdateWallet(towerCost);

        towerBought.OnDropTower -= DropTower;
        currentlyHoldingTower = false;
        towerBought = null;
    }

    //Method called by UI button
    public void UpdateTower()
    {
        if(GameManager.Instance.IsGamePaused || 
            !towerSelected || 
            !towerSelected.HasUpgrade) return;
        
        foreach(StoreButton b in allStoreButtons)
        {
            if (b.Button.Name != "Update") continue; 

            if (!b.CanClick) return;
            
            Wallet.Instance.UpdateWallet(-b.Button.Cost);
            towerSelected.UpdateTower();
            
            if (b.gameObject.TryGetComponent<ToggleObjUI>(out var toggler)) toggler.ManuallyDisableElement();

            SwitchToBuy();
        }
    }

    //Method called by UI button
    public void RemoveTower()
    {
        if (GameManager.Instance.IsGamePaused) return;

        towerSelected.Placer.TowerSpot.TowerLeft();
        Destroy(towerSelected.gameObject);

        SwitchToBuy();
    }

    //Method called by UI buttons
    public void PurchaseType(SO_Tower toBuy)
    {
        if (GameManager.Instance.IsGamePaused || 
            currentlyHoldingTower) return;

        if (toBuy.Name == "")
        {
            Debug.LogWarning("No name assigned to tower purchased attempt!");
            return;
        }

        foreach(StoreButton button in allStoreButtons)
        {
            //find out what button was clicked
            if (toBuy.Name != button.Button.Name) continue;

            //no money
            if (!button.CanClick) return;

            Wallet.Instance.UpdateWallet(-button.Button.Cost);
            towerCost = button.Button.Cost;
            break;
        }

        //check if we have the amount of money required
        towerBought = Instantiate(toBuy.TowerPrefab);
        towerBought.OnDropTower += DropTower;   
        towerBought.SetBought();
        currentlyHoldingTower = true;
        OnPurchasedTower?.Invoke();
    }

    private void OnDestroy()
    {
        TowerSelector.OnTowerWantsUpdate -= SwitchToUpdate;
        TowerSelector.OnNoUpdateNeeded -= SwitchToBuy;
    }

}