using UnityEngine;

[RequireComponent(typeof(MeshRenderer),typeof(MeshFilter), typeof(BoxCollider))]
public class TowerSpot : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;

    private MeshRenderer mesh;
    private BoxCollider colliderBox;

    public bool Occupied { get; private set; }

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();   
        colliderBox = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        Occupied = false;

        Store.OnPurchasedTower += EnableAvailableMarker;
        TowerPlacer.OnTowerPlaced2 += DisableAvailableMarker;
    }

    private void SetTowerSpotState(bool state)
    {
        mesh.enabled = state;
        colliderBox.enabled = state;
    }

    private void EnableAvailableMarker()
    {
        if (Occupied) return;
        arrowPrefab.SetActive(true);
    }

    private void DisableAvailableMarker() => arrowPrefab.SetActive(false);

    public void TowerPlaced()
    {
        if (Occupied) return;
        Occupied = true;
        SetTowerSpotState(false);
    }

    public void TowerLeft()
    {
        Occupied = false;
        SetTowerSpotState(true);
    }
}
