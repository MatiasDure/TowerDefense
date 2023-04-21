using UnityEngine;

///<summary>
/// A class representing an available spot where towers can be placed.
///</summary>
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(BoxCollider))]
public class TowerSpot : MonoBehaviour
{
    ///<summary>
    /// The arrow prefab that is displayed above a tower spot when it is available for placement.
    ///</summary>
    [SerializeField] private GameObject _arrowPrefab;

    ///<summary>
    /// Gets a value indicating whether the tower spot is currently occupied by a tower.
    ///</summary>
    public bool Occupied { get; private set; }

    private MeshRenderer _mesh;
    private BoxCollider _colliderBox;

    private void Awake()
    {
        GetComponents();
    }

    private void GetComponents()
    {
        _mesh = GetComponent<MeshRenderer>();
        _colliderBox = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        Occupied = false;

        Store.OnPurchasedTower += EnableAvailableMarker;
        TowerPlacer.OnTowerDroppedUiChange += DisableAvailableMarker;
    }

    ///<summary>
    /// Sets the state of the tower spot (i.e. enabled or disabled).
    ///</summary>
    ///<param name="state">The state to set the tower spot to.</param>
    private void SetTowerSpotState(bool state)
    {
        _mesh.enabled = state;
        _colliderBox.enabled = state;
    }

    ///<summary>
    /// Enables the arrow prefab above the tower spot when it is available for placement.
    ///</summary>
    private void EnableAvailableMarker()
    {
        if (Occupied || 
            _arrowPrefab == null) return;

        _arrowPrefab.SetActive(true);
    }

    ///<summary>
    /// Disables the arrow prefab above the tower spot when it is not available for placement.
    ///</summary>
    private void DisableAvailableMarker() => _arrowPrefab.SetActive(false);

    ///<summary>
    /// Marks the tower spot as occupied by a tower.
    ///</summary>
    public void TowerPlaced()
    {
        if (Occupied) return;
        Occupied = true;
        SetTowerSpotState(false);
    }

    ///<summary>
    /// Marks the tower spot as unoccupied by a tower.
    ///</summary>
    public void TowerLeft()
    {
        Occupied = false;
        SetTowerSpotState(true);
    }
}

