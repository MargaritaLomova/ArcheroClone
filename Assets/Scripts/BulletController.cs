using System.Threading.Tasks;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField]
    private float _maxLifeTime = 5f;
    [SerializeField]
    private string _targetTag;

    public int Damage { get; set; }
    public bool IsDestroyed { get; private set; }

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(_targetTag))
            Destroy(gameObject);
    }

    private async void StartMoveBullet()
    {
        while (!IsDestroyed)
        {
            transform.Translate(transform.forward, Space.World);

            _currentLifeTime += Time.fixedDeltaTime;
            await Task.Delay((int)(Time.fixedDeltaTime * 2 * 1000));

            if (_currentLifeTime >= _maxLifeTime && !IsDestroyed)
                Destroy(gameObject);
        }
    }
}