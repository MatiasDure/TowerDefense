using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(SphereCollider), typeof(CanonController))]
public class EstablishTarget : MonoBehaviour
{
    private const string TAG_TO_COMPARE = "Enemy";

    private TowerManager manager;
    private SphereCollider sphereCollider;

    private bool canLookForTargets;

    private Dictionary<GameObject,Enemy> targetsDictionary;

    private List<GameObject> targetsToRemove;

    public CanonController CanonController { get; private set; }

    private void Awake()
    {
        targetsDictionary = new(); 
        targetsToRemove = new();
        sphereCollider = GetComponent<SphereCollider>();
        CanonController = GetComponent<CanonController>();
        manager = transform.parent.GetComponent<TowerManager>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        if(manager.IsUpgrade) canLookForTargets = true;
        else
        {
            canLookForTargets = false;
            manager.OnDropTower += SetCanLookForTarget;
        }
    }

    private void LateUpdate()
    {
        //Removing all disabled elements from the dictionary at lateUpdate to avoid updating dictionary when iterating through it
        if (targetsToRemove.Count > 0)
        {
            foreach (var targetToRemove in targetsToRemove)
            {
                if (targetsDictionary.ContainsKey(targetToRemove))
                {
                    targetsDictionary[targetToRemove].OnDisabled -= TargetKilled;
                    targetsDictionary.Remove(targetToRemove);
                }
            }
            targetsToRemove.Clear();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TAG_TO_COMPARE) && 
            !targetsDictionary.ContainsKey(other.gameObject))
        {
            AddTarget(other.gameObject);
            AssignTarget();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TAG_TO_COMPARE)) RemoveTarget(other.gameObject);
    }

    private void AssignTarget()
    {
        if (!canLookForTargets) return;
        CanonController.AssignTarget(targetsDictionary);
    }

    private void TargetKilled(GameObject enemyKilled) => RemoveTarget(enemyKilled);

    private void RemoveTarget(GameObject objectToRemove)
    {
        if (targetsDictionary.ContainsKey(objectToRemove)) targetsToRemove.Add(objectToRemove);
        else throw new Exception("Object to remove not found!");
    }

    private void AddTarget(GameObject other)
    {
        targetsDictionary.Add(other, EnemyManager.Instance.FindEnemyInScene(other));
        targetsDictionary[other].OnDisabled += TargetKilled;
    }

    private void SetCanLookForTarget()
    {
        canLookForTargets = true;
        AssignTarget();
    }
    
    public void SetRange(float range) => sphereCollider.radius = range;

    private void OnDestroy()
    {
        if(!manager.IsUpgrade) manager.OnDropTower -= SetCanLookForTarget;
    }
}
