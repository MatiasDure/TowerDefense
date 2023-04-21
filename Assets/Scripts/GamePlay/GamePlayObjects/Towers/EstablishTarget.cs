using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Finds the enemies in range for each tower
/// </summary>
/// <remarks> Found enemies are then passed to the CannonController for further processing </remarks>
[RequireComponent(typeof(SphereCollider), typeof(CannonController))]
public class EstablishTarget : MonoBehaviour
{
    private const string TAG_TO_COMPARE = "Enemy";
    
    [SerializeField] private GameObject _rangeIndicator;

    private TowerManager _manager;
    private SphereCollider _sphereCollider;
    private Dictionary<GameObject,Enemy> _targetsDictionary = new();
    private List<GameObject> _targetsToRemove = new();
    private bool _canLookForTargets;

    /// <summary>
    /// Gets the cannon controller associated with this tower instance
    /// </summary>
    public CannonController Cannon { get; private set; }

    private void Awake() => GetComponents();
    
    private void Start() => InitCanLookForTargets();

    private void LateUpdate() => RemoveTargets();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TAG_TO_COMPARE) && 
            !_targetsDictionary.ContainsKey(other.gameObject))
        {
            AddTarget(other.gameObject);
            PassTargets();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TAG_TO_COMPARE)) SetRemoveTarget(other.gameObject);
    }

    private void OnDestroy()
    {
        if (!_manager.IsUpgrade) _manager.OnDropTower -= SetCanLookForTarget;
    }

    private void GetComponents()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        Cannon = GetComponent<CannonController>();
        _manager = transform.parent.GetComponent<TowerManager>();
    }

    /// <summary>
    /// Initializes the tower's ability to look for targets based on whether it has been upgraded or not.
    /// If the tower is an upgraded tower, it can immediately look for targets.
    /// If it hasn't been upgraded, it cannot look for targets until it is dropped after purchase.
    /// </summary>
    private void InitCanLookForTargets()
    {
        if (_manager.IsUpgrade) _canLookForTargets = true;
        else
        {
            _canLookForTargets = false;
            _manager.OnDropTower += SetCanLookForTarget;
        }
    }

    /// <summary>
    /// Removes all the targets that have either died or left the range
    /// </summary>
    private void RemoveTargets()
    {
        if (_targetsToRemove.Count > 0)
        {
            foreach (var targetToRemove in _targetsToRemove)
            {
                if (_targetsDictionary.ContainsKey(targetToRemove))
                {
                    _targetsDictionary[targetToRemove].OnNotTargetable -= SetRemoveTarget;
                    _targetsDictionary.Remove(targetToRemove);
                }
            }
            _targetsToRemove.Clear();
        }
    }

    /// <summary>
    /// Sends the targets to the tower's cannon controller component
    /// </summary>
    private void PassTargets()
    {
        if (!_canLookForTargets) return;
        Cannon.AssignTarget(_targetsDictionary);
    }

    /// <summary>
    /// Sets the targets that need to be removed during the late update call
    /// </summary>
    /// <param name="objectToRemove"> The enemy that needs to be removed from the current targets </param>
    private void SetRemoveTarget(GameObject objectToRemove)
    {
        if (_targetsDictionary.ContainsKey(objectToRemove)) _targetsToRemove.Add(objectToRemove);
        else throw new Exception("Object to remove not found!");
    }

    /// <summary>
    /// Adds a target GameObject to the target dictionary and subscribes to its OnDisabled event.
    /// </summary>
    /// <param name="other">The GameObject to add as a target.</param>
    private void AddTarget(GameObject other)
    {
        _targetsDictionary.Add(other, EnemyManager.Instance.FindEnemyInScene(other));
        _targetsDictionary[other].OnNotTargetable += SetRemoveTarget;
    }

    /// <summary>
    /// Sets whether the tower can look for targets and passes any existing targets to the cannon manager.
    /// </summary>
    /// <param name="isSuccess">Whether the tower was placed successfully after its purchased</param>
    private void SetCanLookForTarget(bool isSuccess)
    {
        if (!isSuccess) return;
        _canLookForTargets = true;
        PassTargets();
    }

    /// <summary>
    /// Sets the range of the tower and updates the range indicator if one is available.
    /// </summary>
    /// <param name="range">The new range of the tower.</param>
    public void SetRange(float range)
    {
        _sphereCollider.radius = range;

        if (_rangeIndicator == null) return;
        _rangeIndicator.transform.localScale = new Vector3(range, 1, range);
    }
}
