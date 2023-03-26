using System;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    private const string TAG_TO_COMPARE = "Placeable";

    TowerManager manager;

    public TowerSpot TowerSpot { get; private set; }

    public event Action<bool> OnTowerPlaced;
    public static event Action OnTowerPlaced2;

    private void Awake() => manager = GetComponent<TowerManager>();

    private void Update()
    {
        if (manager.CurrentTowerStatus != TowerManager.TowerStatus.Holding) return;

        if (InputManager.Instance.ClickedLeftMouse) DropTower();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (manager == null) return;
        if (manager.CurrentTowerStatus == TowerManager.TowerStatus.Holding &&
            other.CompareTag(TAG_TO_COMPARE))
        {
            TowerSpot spot = other.gameObject.GetComponent<TowerSpot>();
            if (!spot.Occupied) TowerSpot = spot;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (manager == null) return;
        if (manager.CurrentTowerStatus == TowerManager.TowerStatus.Holding &&
            other.CompareTag(TAG_TO_COMPARE) &&
            TowerSpot.gameObject == other.gameObject) TowerSpot = null;
    }

    private void DropTower()
    {
        bool success = false;

        if (TowerSpot != null)
        {
            this.transform.position = TowerSpot.gameObject.transform.position;
            TowerSpot.TowerPlaced();
            success = true;
        }
        else if (manager.CurrentTowerStatus == TowerManager.TowerStatus.Holding) Destroy(gameObject);

        OnTowerPlaced?.Invoke(success);
        OnTowerPlaced2?.Invoke();
    }

    public void SetTowerSpot(TowerSpot spot) => TowerSpot = spot;
}
