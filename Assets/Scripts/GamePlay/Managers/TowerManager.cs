using System;
using UnityEngine;

[RequireComponent(typeof(TowerSelector), typeof(TowerPlacer))]
public class TowerManager : MonoBehaviour
{
    public enum TowerStatus
    {
        Holding,
        Active
    }
    
    private readonly Vector3 TOWER90DEG = new(0, 90, 0);

    [SerializeField] private SO_Tower towerInfo;
    [SerializeField] private Upgrador upgradeFactory;
    [SerializeField] private MouseFollower mouseFollower;
    [SerializeField] private EstablishTarget targetManager;

    public bool IsUpgrade { get; private set; }
    public TowerPlacer Placer { get; private set; }
    public TowerSelector TowerSelector { get; private set; }
    public TowerStatus CurrentTowerStatus { get; private set; }
    public bool HasUpgrade { get; private set; }

    public event Action<bool> OnDropTower;

    private void Awake()
    {
        TowerSelector = GetComponent<TowerSelector>();
        Placer = GetComponent<TowerPlacer>();
        
        if(!targetManager) targetManager = GetComponentInChildren<EstablishTarget>();

        HasUpgrade = upgradeFactory != null;
    }

    private void Start()
    {
        PassInfoToComponents();
        Placer.OnTowerPlaced += DropTower;
    }

    private void Update()
    {
        //selecting tower for update/removal
        if(CurrentTowerStatus == TowerStatus.Active)
        {
            if(InputManager.Instance.ClickedLeftMouse) TowerSelector.CheckSelect();
            return;
        }

        //tower is still being held after purchase
        if(InputManager.Instance.ClickedRightMouse) RotateHoldingTower();
    }

    private void OnMouseDown()
    {
        if (CurrentTowerStatus == TowerStatus.Active) TowerSelector.Selected();
    }

    private void PassInfoToComponents()
    {
        if (HasUpgrade) upgradeFactory.SetUpgradedVersion(towerInfo.UpgradedTower);
        if (targetManager)
        {
            targetManager.SetRange(towerInfo.Range);
            targetManager.CanonController.SetDamageAmount(towerInfo.AmountInflict);
            targetManager.CanonController.SetShootDelay(towerInfo.CooldownShootTimer);
        }
    }

    private void DropTower(bool isSuccess)
    {
        Cursor.visible = true;
        CurrentTowerStatus = TowerStatus.Active;
        
        OnDropTower?.Invoke(isSuccess);
        if ( mouseFollower != null ) mouseFollower.UpdateFollowingMouse(false);
    }

    private void RotateHoldingTower()
    {
        this.transform.Rotate(TOWER90DEG);
        if(targetManager != null) targetManager.CanonController.ModifyIdleRotation(this.transform.localRotation);
    }

    private void ReplaceTowerWithUpdated(TowerManager upgradedVersion) => upgradedVersion.PassTower(Placer.TowerSpot);

    public void SetBought() => CurrentTowerStatus = TowerStatus.Holding;

    public void TagAsUpgrade()
    {
        IsUpgrade = true;
        CurrentTowerStatus = TowerStatus.Active;
    }

    public void UpdateTower()
    {
        if (upgradeFactory == null) return;
        TowerManager upgradedVersionInstance = upgradeFactory.CreateUpdatedTower();
        upgradedVersionInstance.TagAsUpgrade();
        if (upgradedVersionInstance != null)
        {
            ReplaceTowerWithUpdated(upgradedVersionInstance);
            Destroy(this.gameObject);
        }
    }

    public void PassTower(TowerSpot towerSpot) => Placer.SetTowerSpot(towerSpot);
}

