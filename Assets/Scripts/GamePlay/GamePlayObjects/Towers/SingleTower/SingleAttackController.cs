using System.Linq;
using UnityEngine;

/// <summary>
/// Debuff tower type
/// </summary>
public class SingleAttackController : CannonController
{
    /// <summary>
    /// Inflicts damage on enemies within range once at a time
    /// </summary>
    /// <remarks>
    /// <para> Method inherited from CanonController </para>
    /// <para> 
    /// Locks on the first enemy that entered this tower's range, 
    /// and doesn't unlock until that enemy is no longer in range or 
    /// has been eliminated
    /// </para>
    /// </remarks>
    protected override void Shoot()
    {
        GameObject target = Targets.Keys.FirstOrDefault();
        TargetToLookAt = target.transform;

        if (!LockedOnTarget(target)) return;

        Targets[target].TakeDamage(InflictAmount);
        CreateBullet(target.transform);
    }
}
