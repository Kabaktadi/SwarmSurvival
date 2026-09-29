using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileConfig _config;
    
    private IObjectPool<Projectile> _objectPool;
    
    public IObjectPool<Projectile> ObjectPool { set => _objectPool = value; }

    public void Deactivate()
    {
        StartCoroutine(DeactivateRoutine(_config.TimeoutDelay));
    }

    IEnumerator DeactivateRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        
        _objectPool.Release(this);
    }
}
