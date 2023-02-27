using System;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField]
    private int _bulletDamage;
    [SerializeField]
    private float _timeBetweenShooting = 2f;

    public Action OnMoveAction { get; set; }

    private PlayerControl _inputControls;

    private List<BaseEnemyController> _currentEnemies = new List<BaseEnemyController>();

    private int _health;

    private bool _isMove;
    private bool _isDead;

    private void Awake()
    {
        _inputControls = new PlayerControl();
        _health = _startHealth;
        _playerHealth.UpdateText(_health, false);

        GetCurrentEnemies();
        if (_currentEnemies != null && _currentEnemies.Count > 0)
            Helpers.Delay(1f, () => Shoot());

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

    private void OnDestroy()
    {
        _isDead = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("EnemyBullet"))
        {
            _animator.SetTrigger("Hit");

            var currentDamage = collider.gameObject.GetComponent<BulletController>().Damage;
            _health -= currentDamage;
            _playerHealth.UpdateText(_health);
            if (_health <= 0)
                Death();
        }
    }

    public void RemoveDeadEnemy(BaseEnemyController deadEnemy)
    {
        _currentEnemies.Remove(deadEnemy);
        SortEnemiesByDistance();
    }

    public void SortEnemiesByDistance()
    {
        if (_currentEnemies != null && _currentEnemies.Count > 0)
            _currentEnemies = _currentEnemies.OrderBy(enemy => Vector2.Distance(transform.position, enemy.transform.position)).Reverse().ToList();
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

        SortEnemiesByDistance();
        if (_currentEnemies != null && _currentEnemies.Count > 0)
            Shoot();

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

    private void GetCurrentEnemies()
    {
        _currentEnemies = FindObjectsOfType<BaseEnemyController>().ToList();
        SortEnemiesByDistance();
    }

    private void Shoot()
    {
        Helpers.Delay(() => _isMove || _currentEnemies.Count == 0, null, () =>
        {
            var aliveEnemies = _currentEnemies.Where(enemy => !enemy.IsDead).ToList();
            if (!_isDead && aliveEnemies != null && aliveEnemies.Count > 0)
            {
                transform.LookAt(aliveEnemies[0].transform);

                _animator.SetTrigger("Shoot");

                var newBullet = Instantiate(_bulletPrefab);
                newBullet.Set(transform, _bulletDamage, aliveEnemies[0].transform);
            }
        }, _timeBetweenShooting);
    }

    private void Death()
    {
        _animator.SetTrigger("Death");
    }
}