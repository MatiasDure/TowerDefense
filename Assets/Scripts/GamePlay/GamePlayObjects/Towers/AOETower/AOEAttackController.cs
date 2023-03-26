using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAttackController : CanonController
{
    protected override void Shoot()
    {
        foreach (var target in Targets)
        {
            target.Value.TakeDamage(inflictAmount);
            targetToLookAt = target.Value.transform;
            audioSource.Play();
        }
    }
}
