using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    [SerializeField] Button[] buttonsToUpdate;
    [SerializeField] StoreButton[] storeButtons;

    private bool toUpdate;
    private bool currentlyHoldingTower;
    private TowerManager towerSelected;
    private TowerManager towerBought;

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
        if (buttonsToUpdate.Length == 0 || storeButtons.Length == 0) return;

        //Toggling between purchase buttons and update/remove buttons        
        if (toUpdate && !buttonsToUpdate[0].gameObject.activeInHierarchy) ToggleButtonsState(true);

        else if (!toUpdate && !storeButtons[0].gameObject.activeInHierarchy) ToggleButtonsState(false);
    }

    private void ToggleButtonsState(bool updateTowerButtonOn)
    {
        for (int i = 0; i < storeButtons.Length; i++) storeButtons[i].gameObject.SetActive(!updateTowerButtonOn);
        
        for (int i = 0; i < buttonsToUpdate.Length; i++) buttonsToUpdate[i].gameObject.SetActive(updateTowerButtonOn);
    }

    private void SwitchToUpdate(TowerSelector tower)
    {
        //if two towers are selected, disable the first selected tower
        if (towerSelected != null && towerSelected.gameObject != tower.gameObject) towerSelected.TowerSelector.Selected();
        toUpdate = true;
        towerSelected = tower.gameObject.GetComponent<TowerManager>();
    }

    private void SwitchToBuy()
    {
        towerSelected = null;
        toUpdate = false;
    }

    private void DropTower()
    {
        towerBought.OnDropTower -= DropTower;
        currentlyHoldingTower = false;
        towerBought = null;
    }

    //Method called by UI button
    public void UpdateTower()
    {
        if(GameManager.Instance.IsGamePaused) return;
        if (!towerSelected || !towerSelected.HasUpgrade) return;
        
        foreach(StoreButton b in storeButtons)
        {
            if(b.Button.Name == "Update" && b.CanClick)
            {
                Wallet.Instance.UpdateWallet(-b.Button.Cost);
                towerSelected.UpdateTower();
                SwitchToBuy();
                return;
            }
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
        if (GameManager.Instance.IsGamePaused || currentlyHoldingTower) return;

        if (toBuy.Name == "")
        {
            Debug.LogWarning("No name assigned to tower purchased attempt!");
            return;
        }

        foreach(StoreButton button in storeButtons)
        {
            //find out what button was clicked
            if (toBuy.Name != button.Button.Name) continue;

            //no money
            if (!button.CanClick) return;

            Wallet.Instance.UpdateWallet(-button.Button.Cost);
            break;
        }

        //check if we have the amount of money required
        towerBought = Instantiate(toBuy.TowerPrefab);
        towerBought.OnDropTower += DropTower;   
        towerBought.SetBought();
        currentlyHoldingTower = true;
    }

    private void OnDestroy()
    {
        TowerSelector.OnTowerWantsUpdate -= SwitchToUpdate;
        TowerSelector.OnNoUpdateNeeded -= SwitchToBuy;
    }
}