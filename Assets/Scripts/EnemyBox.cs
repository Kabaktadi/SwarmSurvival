using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBox : MonoBehaviour, IEnemy
{
    [SerializeField] private EnemyConfig _config;
    
    private float _health = 100f;
    private GameObject _player;
    private Camera _camera;
    private Rigidbody _rigidbody;
    private Canvas _healthBarCanvas;
    private Image _healthBar;
    private Renderer _renderer;
    
    public void Initialize()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _healthBarCanvas = GetComponentInChildren<Canvas>();
        _healthBar = _healthBarCanvas.GetComponentsInChildren<Image>()[1];
        _player = GameObject.FindGameObjectWithTag("Player");
        _camera = Camera.main;
        _renderer = GetComponentInChildren<Renderer>();
    }
    
    void Update()
    {
        if (_player != null)
        {
            _rigidbody.transform.position = Vector3.MoveTowards(_rigidbody.transform.position, _player.transform.position, _config.Speed * Time.deltaTime);
        }
        _healthBarCanvas.transform.rotation = Quaternion.LookRotation(_healthBarCanvas.transform.position - _camera.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            other.gameObject.SetActive(false);
            TakeDamage(20f);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            _player.GetComponent<PlayerController>().TakeDamage(_config.Damage);
            TakeDamage(100f);
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
        yield return new WaitForSeconds(0.1f);
        _renderer.material.color = Color.red;
    }
}
