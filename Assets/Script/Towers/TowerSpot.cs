using UnityEngine;

[RequireComponent(typeof(MeshRenderer),typeof(MeshFilter), typeof(BoxCollider))]
public class TowerSpot : MonoBehaviour
{
    private MeshRenderer mesh;
    private BoxCollider colliderBox;

    public bool Occupied { get; private set; }

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();   
        colliderBox = GetComponent<BoxCollider>();
    }

    private void Start() => Occupied = false;

    private void EnableElements(bool state)
    {
        mesh.enabled = state;
        colliderBox.enabled = state;
    }

    public void TowerPlaced()
    {
        if (Occupied) return;
        Occupied = true;
        EnableElements(false);
    }

    public void TowerLeft()
    {
        Occupied = false;
        EnableElements(true);
    }
}
