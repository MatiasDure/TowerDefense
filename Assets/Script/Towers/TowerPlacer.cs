using System;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    private const string TAG_TO_COMPARE = "Placeable";

    TowerManager manager;

    public TowerSpot TowerSpot { get; private set; }

    public event Action OnTowerPlaced;
    public event Action OnPlaceablePlace;
    public event Action OnNotPlaceablePlace;

    private void Awake() => manager = GetComponent<TowerManager>();

    private void Update()
    {
        if (manager.CurrentTowerStatus != TowerManager.TowerStatus.Holding) return;

        if (InputManager.Instance.ClickedLeftMouse) PlaceTower();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (manager == null) return;
        if (manager.CurrentTowerStatus == TowerManager.TowerStatus.Holding &&
            other.CompareTag(TAG_TO_COMPARE))
        {
            TowerSpot spot = other.gameObject.GetComponent<TowerSpot>();
            if (!spot.Occupied)
            {
                TowerSpot = spot;
                OnPlaceablePlace?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (manager == null) return;
        if (manager.CurrentTowerStatus == TowerManager.TowerStatus.Holding &&
            other.CompareTag(TAG_TO_COMPARE) &&
            TowerSpot.gameObject == other.gameObject)
        {
            TowerSpot = null;
            OnNotPlaceablePlace?.Invoke();
            return;
        }
    }

    private void PlaceTower()
    {
        if (TowerSpot != null)
        {
            this.transform.position = TowerSpot.gameObject.transform.position;
            TowerSpot.TowerPlaced();
        }
        else if (manager.CurrentTowerStatus == TowerManager.TowerStatus.Holding) Destroy(gameObject);

        OnTowerPlaced?.Invoke();
    }

    public void SetTowerSpot(TowerSpot spot) => TowerSpot = spot;
}
