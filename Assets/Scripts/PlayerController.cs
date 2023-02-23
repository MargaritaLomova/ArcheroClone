using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private float _movementSpeed;

    [SerializeField]
    private Animator _animator;

    public Action OnMoveAction { get; set; }

    private PlayerControl _inputControls;

    private bool _isMove;

    private void Awake()
    {
        _inputControls = new PlayerControl();

        _inputControls.Player.Move.performed += OnMove;
        _inputControls.Player.Move.canceled += OnMoveStopped;
    }

    private void OnEnable()
    {
        _inputControls.Enable();
    }

    private void OnDisable()
    {
        _inputControls.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (!_isMove)
        {
            _isMove = true;

            if (!_animator.GetBool("Run"))
                _animator.SetBool("Run", true);

            Movement();
        }
    }

    private void OnMoveStopped(InputAction.CallbackContext context)
    {
        _isMove = false;

        if (_animator.GetBool("Run"))
            _animator.SetBool("Run", false);
    }

    private async void Movement()
    {
        while (_isMove)
        {
            OnMoveAction?.Invoke();

            var inputPosition = _inputControls.Player.Move.ReadValue<Vector2>();

            var movementDirection = new Vector3(inputPosition.x, 0, inputPosition.y) * _movementSpeed * Time.fixedDeltaTime;
            transform.Translate(movementDirection, Space.World);

            if (movementDirection != Vector3.zero)
            {
                var rotationDirection = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationDirection, _rotationSpeed * Time.fixedDeltaTime);
            }
            await Task.Delay((int)(Time.fixedDeltaTime * 1000));
        }
    }
}