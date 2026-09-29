using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerConfig _config;
    
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Camera _camera;
    [SerializeField] private Rigidbody _gun;
    [SerializeField] private Projectile _projectilePrefab;
    
    private Vector3 _inputVector;
    private Vector2 _moveDirection = Vector2.zero;
    private bool _isFiring = false;
    private float _nextTimeToFire;
    private float _health;
    private Canvas _healthBarCanvas;
    private Image _healthBar;
    private Renderer _renderer;
    
    private ObjectPool<Projectile> _projectilePool;

    private void Awake()
    {
        _projectilePool = new ObjectPool<Projectile>(CreateProjectile, OnGetFromPool, OnReleaseFromPool, OnDestroyPoolObject);
    }

    private void Start()
    {
        _health = _config.Health;
        _healthBarCanvas = GetComponentInChildren<Canvas>();
        _healthBar = _healthBarCanvas.GetComponentsInChildren<Image>()[1];
        _renderer = GetComponentInChildren<Renderer>();
    }

    private void Update()
    {
        LookAtMouse();
        _healthBarCanvas.transform.rotation = Quaternion.LookRotation(_healthBarCanvas.transform.position - _camera.transform.position);
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
                projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * _config.ProjectileSpeed, ForceMode.Acceleration);
                projectile.Deactivate();
            }
            
            _nextTimeToFire = Time.time + _config.FireCooldown;
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
        var targetPosition = _rigidbody.transform.position + _inputVector * _config.Speed;
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

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0f)
        {
            Destroy(gameObject);
        }
        else
        {
            _healthBar.transform.localScale = new Vector2(_health / 100f, _healthBar.transform.localScale.y); 
            
            StopAllCoroutines();
            StartCoroutine(Flash());
        }
    }
    
    private IEnumerator Flash()
    {
        _renderer.material.color = Color.white;
        yield return new WaitForSeconds(0.3f);
        _renderer.material.color = Color.lightBlue;
    }
}
