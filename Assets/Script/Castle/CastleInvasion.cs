using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            if (!currentEnemy) other.TryGetComponent(out currentEnemy);
            
            castle.TakeDamage(currentEnemy.Stats.Damage);
            currentEnemy.DisableEnemy();
        }
    }
}
