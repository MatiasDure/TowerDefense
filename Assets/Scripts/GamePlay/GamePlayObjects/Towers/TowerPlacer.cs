using System;
using UnityEngine;

/// <summary>
/// Handles placing towers in available TowerSpots
/// </summary>
public class TowerPlacer : MonoBehaviour
{
    private const string TAG_TO_COMPARE = "Placeable";

    private TowerManager _manager;

    /// <summary>
    /// Gets the spot where the tower is currently placed
    /// </summary>
    public TowerSpot TowerSpot { get; private set; }

    /// <summary>
    /// Event that is triggered when this tower instance has been dropped
    /// </summary>
    /// <remarks> Static event that doesn't care about which tower instance invoked it </remarks>
    public static event Action OnTowerDroppedUiChange;

    /// <summary>
    /// Event that is triggered when this tower instance has been dropped
    /// </summary>
    /// <remarks> Sends a bool indicating whether the drop was successfull or not </remarks>
    public event Action<bool> OnTowerDroppedStatusChange;

    private void Awake() => GetComponents();

    private void GetComponents()
    {
        _manager = GetComponent<TowerManager>();
    }

    /// <summary>
    /// Checks if the tower is currently being held, and if the player has clicked the left mouse button.
    /// If both conditions are met, the tower is dropped.
    /// </summary>
    private void Update()
    {
        if (_manager.CurrentTowerStatus != TowerManager.TowerStatus.Holding) return;

        if (InputManager.Instance.ClickedLeftMouse) DropTower();
    }

    /// <summary>
    /// Called when a collider enters the trigger zone.
    /// If the tower is currently being held and the collider's tag matches the target tag,
    /// the tower spot is set to the unoccupied tower spot that entered the trigger zone.
    /// </summary>
    /// <param name="other">The collider that entered the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (_manager == null) return;

        if (_manager.CurrentTowerStatus == TowerManager.TowerStatus.Holding &&
            other.CompareTag(TAG_TO_COMPARE))
        {
            TowerSpot spot = other.gameObject.GetComponent<TowerSpot>();
            if (!spot.Occupied) TowerSpot = spot;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_manager == null || 
            other == null) return;

        try
        {
            if (_manager.CurrentTowerStatus == TowerManager.TowerStatus.Holding &&
                other.CompareTag(TAG_TO_COMPARE) &&
                TowerSpot.gameObject == other.gameObject) TowerSpot = null;
        }
        catch (Exception e) { Debug.LogWarning(e.Message); }
    }

    /// <summary>
    /// Drops the tower onto the tower spot or destroys it if no tower spot was available
    /// </summary>
    private void DropTower()
    {
        bool success = false;

        if (TowerSpot != null)
        {
            this.transform.position = TowerSpot.gameObject.transform.position;
            TowerSpot.TowerPlaced();
            success = true;
        }
        else if (_manager.CurrentTowerStatus == TowerManager.TowerStatus.Holding) Destroy(gameObject);
        OnTowerDroppedStatusChange?.Invoke(success);
        OnTowerDroppedUiChange?.Invoke();
    }

    public void SetTowerSpot(TowerSpot spot) => TowerSpot = spot;
}
