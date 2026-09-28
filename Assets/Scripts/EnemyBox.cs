using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBox : MonoBehaviour, IEnemy
{
    [SerializeField] private float _speed = 1.5f;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Canvas _healthBarCanvas;
    
    private float _health = 100f;
    private GameObject _player;
    private Camera _camera;
    
    public void Initialize()
    {
        
    }
    
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _camera = Camera.main;
    }
    
    void Update()
    {
        _rigidbody.transform.position = Vector3.MoveTowards(_rigidbody.transform.position, _player.transform.position, _speed * Time.deltaTime);
        _healthBarCanvas.transform.rotation = Quaternion.LookRotation(_healthBarCanvas.transform.position - _camera.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            other.gameObject.SetActive(false);
            TakeDamage(20f);
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
        }
        
    }
}
