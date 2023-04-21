using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AOE tower type
/// </summary>
public class AOEAttackController : CannonController
{
    /// <summary>
    /// Inflicts damage on every enemy within range
    /// </summary>
    /// <remarks> Method inherited from CanonController </remarks>
    protected override void Shoot()
    {
        foreach (var target in Targets)
        {
            target.Value.TakeDamage(InflictAmount);
            CreateBullet(target.Key.transform);
        }

    }
}
