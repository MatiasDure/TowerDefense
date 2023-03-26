using UnityEngine;
using System;

[RequireComponent(typeof(Castle))]
public class CastleInvasion : MonoBehaviour
{
    private Castle castle;

    private void Awake()
    {
        castle = GetComponent<Castle>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Enemy currentEnemy = EnemyManager.Instance.FindEnemyInScene(other.gameObject);

            if (!currentEnemy) throw new Exception("Enemy was not found in Enemy Manager!");
            
            castle.TakeDamage(currentEnemy.Stats.Damage);
            currentEnemy.DisableEnemy();
        }
    }
}
