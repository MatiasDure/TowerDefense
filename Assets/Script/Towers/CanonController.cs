using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CanonController : MonoBehaviour
{
    private enum ShootState
    {
        NotAbleToFire,
        CoolingDown,
        AbleToFire
    }

    private const float MAX_DEG_TO_ROTATE = 5f;
    private const float LERP_SCALE = 0.01f;
    
    [SerializeField] private GameObject canonPivot;
    private TowerManager manager;
    private ShootState currentState = ShootState.AbleToFire;
    private Quaternion idleRot;
    private float shootDelay;

    [SerializeField] protected AudioSource audioSource;
    protected float inflictAmount;
    protected Transform targetToLookAt;

    public Dictionary<GameObject, Enemy> Targets { get; private set; }

    //protected Dictionary<GameObject, Enemy> enemiesSaved;

    private void Awake()
    {
        manager = transform.parent.GetComponent<TowerManager>();
        Targets = new();
        //enemiesSaved = new();
    }

    // Start is called before the first frame update
    private void Start()
    {
        idleRot = transform.rotation;

        if(manager.IsUpgrade) currentState = ShootState.AbleToFire;
        else
        {
            currentState = ShootState.NotAbleToFire;
            manager.OnDropTower += SetCanShoot;
        }
    }
    private void Update()
    {
        if (!TargetAvailable())
        {
            targetToLookAt = null;
            return;
        }

        if (currentState == ShootState.AbleToFire) StartCoroutine(ShootDelay());
    }

    private void FixedUpdate()
    {
        //Check whether there are targets
        if (canonPivot == null) return;
        
        RotateCanon();
    }

    private IEnumerator ShootDelay()
    {
        currentState = ShootState.CoolingDown;
        Shoot();
        yield return new WaitForSeconds(shootDelay);

        currentState = ShootState.AbleToFire;
    }

    private void RotateCanon()
    {
        if (!TargetAvailable())
        {
            ResetCanon();
            return;
        }
        else if (targetToLookAt != null) LookAtLerp(targetToLookAt);
    }

    private void LookAtLerp(Transform toLook)
    {
        Quaternion rotTarg = Quaternion.LookRotation(toLook.position - canonPivot.transform.position);
        canonPivot.transform.rotation = Quaternion.RotateTowards(canonPivot.transform.rotation, rotTarg, MAX_DEG_TO_ROTATE);
    }

    private void SetCanShoot() => currentState = ShootState.AbleToFire;
    private void ResetCanon() => canonPivot.transform.rotation = Quaternion.Lerp(canonPivot.transform.rotation, idleRot, LERP_SCALE);
    private bool TargetAvailable() => Targets.Count > 0;
    protected virtual void Shoot(){}
    
    public void SetShootDelay(float delay) => shootDelay = delay;

    public void SetDamageAmount(float amount) => inflictAmount = amount;

    public void AssignTarget(Dictionary<GameObject, Enemy> targets) => Targets = targets;

    public void ModifyIdleRotation(Quaternion rotation) => idleRot = rotation;

    private void OnDestroy()
    {
        if (!manager.IsUpgrade) manager.OnDropTower -= SetCanShoot;
    }
}
