using System.Threading.Tasks;
using UnityEngine;

public class BaseEnemyController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    protected Animator _animator;

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

    protected PlayerController _player;

    protected int _health;

    protected bool _isDead;

    protected virtual void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _health = _startHealth;
        _isDead = false;

        Shooting();
        LookOnPlayer();
    }

    private void OnDestroy()
    {
        _isDead = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            _animator.SetTrigger("Hit");

            var currentDamage = collision.gameObject.GetComponent<BulletController>().Damage;
            _health -= currentDamage;
            if (_health <= 0)
                Death();
        }
    }

    protected void LookOnPlayer()
    {
        Helpers.Delay(() => _isDead, null, () =>
        {
            if (!_isDead)
                transform.LookAt(_player.transform);
        });
    }

    protected async void Shoot()
    {
        _animator.SetTrigger("Shoot");

        await Task.Delay((int)(0.2f * 1000));

        if (!_isDead)
        {
            var newBullet = Instantiate(_bulletPrefab);
            newBullet.transform.position = transform.position;
            newBullet.transform.rotation = transform.rotation;
            newBullet.Damage = _bulletDamage;
        }
    }

    private void Shooting()
    {
        Helpers.Delay(() => _isDead, null, () => Shoot(), _timeBetweenShooting, true);
    }

    private void Death()
    {
        _isDead = true;
        _animator.SetTrigger("Death");
        Destroy(gameObject, 1f);
    }
}