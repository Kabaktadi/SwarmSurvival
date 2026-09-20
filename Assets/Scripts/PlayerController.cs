using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _turnSpeed = 300.0f;

    public InputAction playerControls;
    
    private Vector3 _inputVector;
    private Vector2 _moveDirection = Vector2.zero;

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        GatherInputVector();
        LookAt();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void GatherInputVector()
    {
        _moveDirection = playerControls.ReadValue<Vector2>();
        _inputVector = new Vector3(_moveDirection.x, 0, _moveDirection.y);
    }

    void LookAt()
    {
        if (_inputVector != Vector3.zero)
        {
            var relativeDirection = (transform.position + _inputVector) - transform.position;
            var rotation = Quaternion.LookRotation(relativeDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _turnSpeed * Time.deltaTime);
        }
    }

    void MovePlayer()
    {
        _rigidbody.MovePosition(transform.position + (transform.forward * _inputVector.magnitude) * _speed * Time.deltaTime);
    }
}
