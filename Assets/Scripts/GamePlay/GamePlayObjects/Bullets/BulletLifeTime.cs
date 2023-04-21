using System.Collections;
using UnityEngine;

/// <summary>
/// This class is in charge of destroying an insantiated bullet after n amount of seconds
/// if it hasn't been destroyed by then
/// </summary>
public class BulletLifeTime : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    // Start is called before the first frame update
    void Start() => StartCoroutine(LifeTime());

    /// <summary>
    /// A coroutine that waits for n seconds until it destroys this object
    /// </summary>
    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(_lifeTime);
        
        Destroy(gameObject);    
    }
}
