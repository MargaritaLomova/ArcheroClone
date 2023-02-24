using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Animator _animator;

    [Space]
    [Header("Variables")]
    [SerializeField]
    private PlayerHealthController _playerHealth;
    [SerializeField]
    private PlayerMoneyController _playerMoney;

    [Space]
    [Header("Prefabs")]
    [SerializeField]
    private BulletController _bulletPrefab;

    [Space]
    [Header("Variables")]
    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    private int _startHealth = 100;

    public Action OnMoveAction { get; set; }

    private PlayerControl _inputControls;

    private int _health;

    private bool _isMove;

    private void Awake()
    {
        _inputControls = new PlayerControl();
        _health = _startHealth;
        _playerHealth.UpdateText(_health, false);

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            _animator.SetTrigger("Hit");

            var currentDamage = collision.gameObject.GetComponent<BulletController>().Damage;
            _health -= currentDamage;
            _playerHealth.UpdateText(_health);
            if (_health <= 0)
                Death();
        }
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

    private void Death()
    {
        _animator.SetTrigger("Death");
    }
}