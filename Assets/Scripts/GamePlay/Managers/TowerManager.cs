using System;
using UnityEngine;

/// <summary>
/// This class is responsible for managing the tower and its components
/// </summary>
[RequireComponent(typeof(TowerSelector), typeof(TowerPlacer))]
public class TowerManager : MonoBehaviour
{
    /// <summary>
    /// States whether a tower is being held after purchase or is active on the field
    /// </summary>
    public enum TowerStatus
    {
        Holding,
        Active
    }
    
    private readonly Vector3 DEG_90 = new(0, 90, 0);

    [SerializeField] private SO_Tower _towerInfo;
    [SerializeField] private Upgrador _upgradeFactory;
    [SerializeField] private MouseFollower _mouseFollower;
    [SerializeField] private EstablishTarget _targetManager;

    /// <summary>
    /// Indicates whether this instance is an upgrade
    /// </summary>
    public bool IsUpgrade { get; private set; }

    /// <summary>
    /// Indicates whether this instance has an upgrade
    /// </summary>
    public bool HasUpgrade { get; private set; }

    /// <summary>
    /// Gets this instance's <c>TowerPlacer</c> script, which manages the placement of the tower.
    /// </summary>
    public TowerPlacer Placer { get; private set; }

    /// <summary>
    /// Gets this instance's <c>TowerSelector</c> script, which manages the selection of the tower.
    /// </summary>
    public TowerSelector TowerSelector { get; private set; }

    /// <summary>
    /// Gets an enum value indicating the current status of the tower, either "Holding" or "Active".
    /// </summary>
    public TowerStatus CurrentTowerStatus { get; private set; }

    /// <summary>
    /// An event that is triggered when a tower is dropped.
    /// </summary>
    public event Action<bool> OnDropTower;

    private void Awake()
    {
        GetComponents();
        CheckUpdateAvailable();
    }

    private void Start()
    {
        PassInfoToComponents();
        Placer.OnTowerDroppedStatusChange += DropTower;
    }

    private void Update()
    {
        //selecting tower for update/removal
        if (CurrentTowerStatus == TowerStatus.Active)
        {
            if(InputManager.Instance.ClickedLeftMouse) TowerSelector.CheckSelect();
            return;
        }

        //tower is still being held after purchase
        if (InputManager.Instance.ClickedRightMouse) RotateHoldingTower();
    }

    /// <summary>
    /// Handles tower selection.
    /// </summary>
    private void OnMouseDown()
    {
        if (CurrentTowerStatus == TowerStatus.Active) TowerSelector.Selected();
    }

    /// <summary>
    /// Checks and sets if an upgrade is available.
    /// </summary>
    private void CheckUpdateAvailable() => HasUpgrade = _upgradeFactory != null;

    private void GetComponents()
    {
        TowerSelector = GetComponent<TowerSelector>();
        Placer = GetComponent<TowerPlacer>();

        if (!_targetManager) _targetManager = GetComponentInChildren<EstablishTarget>();
    }

    /// <summary>
    ///  Sets the information for the components of the tower
    /// </summary>
    private void PassInfoToComponents()
    {
        if (HasUpgrade) _upgradeFactory.SetUpgradedVersion(_towerInfo.UpgradedTower);

        if (_targetManager == null) return;
        
        _targetManager.SetRange(_towerInfo.Range);
        _targetManager.Cannon.SetInflictAmount(_towerInfo.AmountInflict);
        _targetManager.Cannon.SetShootDelay(_towerInfo.CooldownShootTimer);
    }

    /// <summary>
    /// Handles the tower drop event
    /// </summary>
    /// <param name="isSuccess"> Whether the placement was successfull or not </param>
    private void DropTower(bool isSuccess)
    {
        Cursor.visible = true;
        CurrentTowerStatus = TowerStatus.Active;
        
        OnDropTower?.Invoke(isSuccess);

        if (_mouseFollower != null) _mouseFollower.UpdateFollowingMouse(false);
    }

    /// <summary>
    /// Rotates the tower by 90 degrees when it is being held.
    /// </summary>
    private void RotateHoldingTower()
    {
        this.transform.Rotate(DEG_90);

        if (_targetManager != null) _targetManager.Cannon.InitialIdleRotation(this.transform.localRotation);
    }

    /// <summary>
    /// Replaces the current tower with an upgraded version.
    /// </summary>
    /// <param name="upgradedVersion"> The upgraded tower prefab to instantiate </param>
    private void ReplaceTowerWithUpdated(TowerManager upgradedVersion) => upgradedVersion.PassTower(Placer.TowerSpot);

    /// <summary>
    /// Sets the current status of the tower to "Holding".
    /// </summary>
    public void SetBought() => CurrentTowerStatus = TowerStatus.Holding;

    /// <summary>
    /// Tags this tower instance as an upgrade
    /// </summary>
    public void TagAsUpgrade()
    {
        IsUpgrade = true;
        CurrentTowerStatus = TowerStatus.Active;
    }

    /// <summary>
    /// Creates an upgraded version of the tower and replaces the current tower with it.
    /// </summary>
    public void UpdateTower()
    {
        if (_upgradeFactory == null) return;

        TowerManager upgradedVersionInstance = _upgradeFactory.CreateUpdatedTower();
        upgradedVersionInstance.TagAsUpgrade();
        
        if (upgradedVersionInstance != null)
        {
            ReplaceTowerWithUpdated(upgradedVersionInstance);
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Assigns a placeable spot to the <c>TowerPlacer</c> scipt of this instance
    /// </summary>
    /// <param name="towerSpot"> The placeable spot to pass </param>
    public void PassTower(TowerSpot towerSpot) => Placer.SetTowerSpot(towerSpot);
}

