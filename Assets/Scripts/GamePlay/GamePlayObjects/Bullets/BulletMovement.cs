using UnityEngine;

/// <summary>
/// This class handle bullet movement
/// </summary>
public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 30f;

    private void Update() => Move();

    /// <summary>
    /// Moves the bullet game object forward by at a certain speed
    /// </summary>
    private void Move() => this.transform.position += this.transform.forward * Time.deltaTime * _speed;
}
