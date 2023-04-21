using UnityEngine;

/// <summary>
/// This class is in charge bullets' collision checks
/// </summary>
public class BulletCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
            
        Destroy(this.gameObject);
    }
}
