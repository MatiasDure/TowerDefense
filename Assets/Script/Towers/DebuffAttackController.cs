using System.Collections.Generic;
using UnityEngine;

public class DebuffAttackController : CanonController
{
    protected override void Shoot()
    {
        //note: the same debuff cannot affect the same enemy twice, but another debuff instance could affect
        //that same enemy
        foreach (KeyValuePair<GameObject, Enemy> pair in Targets)
        {
            if (pair.Value.ComparePreviousDebuffers(this)) continue;
            pair.Value.UpdateSpeed(this, pair.Value.Follower.Speed * inflictAmount);
            targetToLookAt = pair.Value.transform;
            audioSource.Play();
            return;
        }
    }
}
