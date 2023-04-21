using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract class that represents how a tower affects enemies within range
/// </summary>
public abstract class CannonController : MonoBehaviour
{
    /// <summary>
    /// Used to help determine in what state a cannon is currently in
    /// </summary>
    private enum ShootState
    {
        NotAbleToFire,
        CoolingDown,
        AbleToFire
    }

    private const float MAX_DEG_TO_ROTATE = 300f;
    private const float LERP_SCALE = 1f;
    private const float RAY_MAX_DIST = 15f;
    private readonly Vector3 BULLET_OFFSET = new Vector3(0, 1.4f, 0);

    [SerializeField] private GameObject _cannonPivot;
    [SerializeField] protected GameObject BulletPrefab;

    private TowerManager _manager;
    private ShootState _currentState = ShootState.AbleToFire;
    private Quaternion _idleRot;
    private float _shootDelay;

    protected float InflictAmount;
    protected Transform TargetToLookAt;
    protected Dictionary<GameObject, Enemy> Targets = new();

    private void Awake() => GetComponents();

    private void Start()
    {
        _idleRot = transform.rotation;

        if(_manager.IsUpgrade) _currentState = ShootState.AbleToFire;
        else
        {
            _currentState = ShootState.NotAbleToFire;
            _manager.OnDropTower += SetCanShoot;
        }
    }

    private void Update()
    {
        if (!TargetAvailable())
        {
            TargetToLookAt = null;
            return;
        }

        if (CheckAbleToShoot()) StartCoroutine(ShootDelay());
    }

    private void FixedUpdate()
    {
        if (_cannonPivot == null) return;
        
        RotateCanon();
    }

    private void OnDestroy()
    {
        if (!_manager.IsUpgrade) _manager.OnDropTower -= SetCanShoot;
    }

    private void GetComponents() => _manager = transform.parent.GetComponent<TowerManager>();

    /// <summary>
    /// Checks if a cannon can start shooting
    /// </summary>
    /// <returns> True if the state is set to allow shooting, otherwise false </returns>
    private bool CheckAbleToShoot() => _currentState == ShootState.AbleToFire;

    /// <summary>
    /// Coroutine which is in charge of keeping track of the cooldown time of the cannons
    /// </summary>
    /// <returns> Suspens the coroutine for an x amount of time </returns>
    private IEnumerator ShootDelay()
    {
        _currentState = ShootState.CoolingDown;
        Shoot();

        yield return new WaitForSeconds(_shootDelay);

        _currentState = ShootState.AbleToFire;
    }

    /// <summary>
    /// Rotates pivot to look at the current target
    /// </summary>
    private void RotateCanon()
    {
        if (!TargetAvailable())
        {
            ResetCannonRotation();
            return;
        }
        else if (TargetToLookAt != null) LookAtLerp(TargetToLookAt);
    }

    /// <summary>
    /// Lerps the rotation towards an enemy in range
    /// </summary>
    /// <param name="toLook"> The transform of the enemy to look at </param>
    private void LookAtLerp(Transform toLook)
    {
        Quaternion rotTarg = Quaternion.LookRotation(toLook.position - _cannonPivot.transform.position);
        _cannonPivot.transform.rotation = Quaternion.RotateTowards(_cannonPivot.transform.rotation, rotTarg, MAX_DEG_TO_ROTATE * Time.deltaTime);
    }

    /// <summary>
    /// Sets the state of the cannon as able to fire
    /// </summary>
    /// <param name="isSucces"> Whether the tower purchased was successfully placed or discarded </param>
    private void SetCanShoot(bool isSucces)
    {
        if (!isSucces) return;
        _currentState = ShootState.AbleToFire;
    }
    
    /// <summary>
    /// Resets the rotation of the cannon
    /// </summary>
    private void ResetCannonRotation() => _cannonPivot.transform.rotation = Quaternion.Lerp(_cannonPivot.transform.rotation, _idleRot, LERP_SCALE * Time.deltaTime);
    
    /// <summary>
    /// Checks if there is a target available
    /// </summary>
    /// <returns> True if there are targets in range, false otherwise </returns>
    private bool TargetAvailable() => Targets.Count > 0;

    /// <summary>
    /// Determines how a cannon affect enemies in range
    /// </summary>
    /// <remarks> Derived classes override this method </remarks>
    protected virtual void Shoot() { }

    /// <summary>
    /// Creates an instance of the bullet prefab to shoot and sets it's pos/rot
    /// </summary>
    protected void CreateBullet(Transform targetToLookAt)
    {
        if (BulletPrefab == null) return;

        GameObject bullet = Instantiate(BulletPrefab);
        bullet.transform.position = this.transform.position + BULLET_OFFSET;
        bullet.transform.LookAt(targetToLookAt);
    }

    /// <summary>
    /// Checks whether the tower is currently looking at the target
    /// passed by shooting a raycast forward
    /// </summary>
    /// <param name="targetToCheck"> The <c>GameObject</c> to check against </param>
    /// <returns> True if one of the hits found was the <c>GameObject</c> looking for, otherwise false. </returns>
    protected bool LockedOnTarget(GameObject targetToCheck)
    {
        Ray ray = new Ray(_cannonPivot.transform.position, _cannonPivot.transform.forward);

        RaycastHit[] hits = Physics.RaycastAll(ray, RAY_MAX_DIST);

        foreach(var hit in hits)
        {
            if (hit.transform.gameObject != targetToCheck) continue;

            return true;
        }

        return false;
    }
    
    /// <summary>
    /// Sets the shooting cooldown time of cannon
    /// </summary>
    /// <param name="delay"> Cooldown time in between each shot </param>
    public void SetShootDelay(float delay) => _shootDelay = delay;

    /// <summary>
    /// Sets the amount this cannon instance inflicts on the enemies
    /// </summary>
    /// <param name="amount"> The amount to set the inflict amount </param>
    /// <remarks> Inflict amount represents multiple things, not only damage amount</remarks>
    /// <example> 
    /// Currently represents damage and speed, but could also represent amount of coins
    /// if we added a tower that steals coins from enemies in range
    /// </example>
    public void SetInflictAmount(float amount) => InflictAmount = amount;

    /// <summary>
    /// Receives all the targets found by the target establish target component
    /// </summary>
    /// <param name="targets"> Targets found </param>
    public void AssignTarget(Dictionary<GameObject, Enemy> targets) => Targets = targets;

    /// <summary>
    /// Sets the initial default rotation of the cannon pivot 
    /// </summary>
    /// <param name="rotation"> Default rotation </param>
    public void InitialIdleRotation(Quaternion rotation) => _idleRot = rotation;
}
