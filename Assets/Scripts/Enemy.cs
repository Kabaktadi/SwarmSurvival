using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 1.5f;
    [SerializeField] private Rigidbody _rigidbody;
    
    private GameObject _player;
    
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    
    void Update()
    {
        _rigidbody.transform.position = Vector3.MoveTowards(_rigidbody.transform.position, _player.transform.position, _speed * Time.deltaTime);
    }
}
