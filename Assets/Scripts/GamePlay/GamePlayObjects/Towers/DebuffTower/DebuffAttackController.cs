using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Debuff tower type
/// </summary>
public class DebuffAttackController : CannonController
{
    /// <summary>
    /// Slows down a single enemy in range
    /// </summary>
    /// <remarks>
    /// <para> Method inherited from CanonController </para>
    /// <para> Only affects enemies which haven't been affected by this debuff instance previously </para>
    /// </remarks>
    protected override void Shoot()
    {
        foreach (var target in Targets)
        {
            if (target.Value.ComparePreviousDebuffers(this)) continue;

            Enemy enemy = target.Value;
            TargetToLookAt = enemy.transform;

            if (!LockedOnTarget(enemy.gameObject)) return;
            
            enemy.UpdateSpeed(this, enemy.Follower.Speed * InflictAmount);
            CreateBullet(enemy.transform);

            break;
        }
    }
}
