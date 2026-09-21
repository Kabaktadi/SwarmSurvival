using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _speed = 0.2f;
    [SerializeField] private float _turnSpeed = 300.0f;
    [SerializeField] private Camera _camera;
    
    private Vector3 _inputVector;
    private Vector2 _moveDirection = Vector2.zero;
    private bool _isFiring = false;

    private void Update()
    {
        LookAtMouse();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        print(_isFiring);
    }

    void LookAtMouse()
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance: 300f))
        {
            var lookPosition = hit.point;
            lookPosition.y = transform.position.y;
            transform.LookAt(lookPosition);
        }
    }

    void MovePlayer()
    {
        var targetPosition = transform.position + _inputVector * _speed;
        transform.position = targetPosition;
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
}
