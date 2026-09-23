using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _speed = 0.2f;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _fireCooldown = 0.1f;
    [SerializeField] private Rigidbody _gun;
    [SerializeField] private float _projectileSpeed = 700f;
    [SerializeField] private Projectile _projectilePrefab;
    
    private Vector3 _inputVector;
    private Vector2 _moveDirection = Vector2.zero;
    private bool _isFiring = false;
    private float _nextTimeToFire;
    
    private ObjectPool<Projectile> _projectilePool;

    private void Awake()
    {
        _projectilePool = new ObjectPool<Projectile>(CreateProjectile, OnGetFromPool, OnReleaseFromPool, OnDestroyPoolObject);
    }

    private void Update()
    {
        LookAtMouse();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        
        if (_isFiring && Time.time >= _nextTimeToFire)
        {
            Projectile projectile = _projectilePool.Get();

            if (projectile != null)
            {
                projectile.transform.SetPositionAndRotation(_gun.position, _gun.rotation);
                projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * _projectileSpeed, ForceMode.Acceleration);
                projectile.Deactivate();
            }
            
            _nextTimeToFire = Time.time + _fireCooldown;
        }
    }

    void LookAtMouse()
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance: 300f))
        {
            var lookPosition = hit.point;
            lookPosition.y = _rigidbody.transform.position.y;
            _rigidbody.transform.LookAt(lookPosition);
        }
    }

    void MovePlayer()
    {
        var targetPosition = _rigidbody.transform.position + _inputVector * _speed;
        _rigidbody.transform.position = targetPosition;
    }

    public void OnMove(InputValue value)
    {
        _moveDirection = value.Get<Vector2>();
        _inputVector = new Vector3(_moveDirection.x, 0, _moveDirection.y);
    }

    public void OnFire(InputValue value)
    {
        _isFiring = value.isPressed;
    }

    public void OnFireRelease(InputValue value)
    {
        _isFiring = value.isPressed;
    }

    private Projectile CreateProjectile()
    {
        Projectile projectileInstance = Instantiate(_projectilePrefab);
        projectileInstance.ObjectPool = _projectilePool;
        return projectileInstance;
    }

    private void OnGetFromPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }
    
    private void OnReleaseFromPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }
    
    private void OnDestroyPoolObject(Projectile projectile)
    {
        if (projectile != null)
        {
            Destroy(projectile.gameObject);
        }
    }
}
