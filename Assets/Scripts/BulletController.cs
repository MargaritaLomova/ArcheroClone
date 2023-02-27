using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField]
    private float _maxLifeTime = 5f;
    [SerializeField]
    private float _movementSpeed = 25f;
    [SerializeField]
    private List<string> _targetTags = new List<string>();

    public int Damage { get; private set; }
    public bool IsDestroyed { get; private set; }

    private Transform _target;
    private float _currentLifeTime;

    private void Awake()
    {
        _currentLifeTime = 0;
        StartMoveBullet();
    }

    private void OnDestroy()
    {
        IsDestroyed = true;
    }

    private void OnApplicationQuit()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (_targetTags.Contains(collider.tag))
            Destroy(gameObject);
    }

    public void Set(Transform shooter, int damage, Transform target = null)
    {
        transform.position = new Vector3(shooter.position.x, shooter.position.y + (shooter.localScale.y * 0.3f), shooter.position.z);
        transform.rotation = shooter.rotation;
        Damage = damage;
        _target = target;
    }

    private async void StartMoveBullet()
    {
        while (!IsDestroyed)
        {
            if (_target == null)
                transform.Translate(transform.forward, Space.World);
            else
                transform.position = Vector3.Lerp(transform.position, _target.position, Time.fixedDeltaTime * _movementSpeed);

            _currentLifeTime += Time.fixedDeltaTime;
            await Task.Delay((int)(Time.fixedDeltaTime * 2 * 1000));

            if (_currentLifeTime >= _maxLifeTime && !IsDestroyed)
                Destroy(gameObject);
        }
    }
}