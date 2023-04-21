using UnityEngine;
using System;

/// <summary>
/// In charge of checking and verifying collisions for the castle
/// </summary>
[RequireComponent(typeof(Castle))]
public class CastleInvasion : MonoBehaviour
{
    private Castle _castle;

    private void Awake()
    {
        _castle = GetComponent<Castle>();
    }

    /// <summary>
    /// Checks for enemies colliding with the castle
    /// </summary>
    /// <param name="other">The object that collided with the castle</param>
    /// <exception cref="NullReferenceException">Thrown when no Enemy scripts have been found on an enemy game object</exception>
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Enemy currentEnemy = EnemyManager.Instance.FindEnemyInScene(other.gameObject);

            if (!currentEnemy) throw new NullReferenceException("Enemy was not found in Enemy Manager!");
            
            //---------------------> raise an event and let the enemy and castle subscribe to this
            _castle.TakeDamage(currentEnemy.Stats.Damage);
            currentEnemy.DisableEnemy();
        }
    }
}
