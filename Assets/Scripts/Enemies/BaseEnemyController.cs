using System.Threading.Tasks;
using UnityEngine;

public class BaseEnemyController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected EnemyAreaController _areaController;

    [Space]
    [Header("Prefabs")]
    [SerializeField]
    protected BulletController _bulletPrefab;

    [Space]
    [Header("Variables")]
    [SerializeField]
    protected int _startHealth;
    [SerializeField]
    protected int _bulletDamage;
    [SerializeField]
    protected float _timeBetweenShooting;
    [SerializeField]
    protected int _costForMurder;

    public bool IsPlayerInEnemyArea => _areaController.IsPlayerInEnemyArea;
    public bool IsDead { get; protected set; }

    protected PlayerController _player;

    protected int _health;

    protected virtual void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _health = _startHealth;
        IsDead = false;

        Shooting();
        LookOnPlayer();
    }

    private void OnDestroy()
    {
        IsDead = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("PlayerBullet"))
        {
            var currentDamage = collider.gameObject.GetComponent<BulletController>().Damage;
            _health -= currentDamage;
            if (_health <= 0)
                Death();
            else
                _animator.SetTrigger("Hit");
        }
    }

    protected void LookOnPlayer()
    {
        Helpers.Delay(() => IsDead, null, () =>
        {
            if (!IsDead)
                transform.LookAt(_player.transform);
        });
    }

    protected async void Shoot()
    {
        if (_areaController.IsPlayerInEnemyArea)
        {
            _animator.SetTrigger("Shoot");

            await Task.Delay((int)(0.2f * 1000));

            if (!IsDead)
            {
                var newBullet = Instantiate(_bulletPrefab);
                newBullet.Set(transform, _bulletDamage);
            }
        }
    }

    private void Shooting()
    {
        Helpers.Delay(() => IsDead, null, () => Shoot(), _timeBetweenShooting, true);
    }

    private void Death()
    {
        _player.RemoveDeadEnemy(this, _costForMurder);
        IsDead = true;
        _animator.SetTrigger("Death");
        Destroy(gameObject, 2f);
    }
}