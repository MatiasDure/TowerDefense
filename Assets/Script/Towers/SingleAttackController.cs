using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingleAttackController : CanonController
{
    protected override void Shoot()
    {
        GameObject target = Targets.Keys.FirstOrDefault();

        Targets[target].TakeDamage(inflictAmount);
        targetToLookAt = target.transform;
        audioSource.Play();
    }
}
