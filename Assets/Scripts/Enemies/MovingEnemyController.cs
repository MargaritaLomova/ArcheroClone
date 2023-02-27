using UnityEngine;

public class MovingEnemyController : BaseEnemyController
{
    [Space]
    [Header("Moving Variables")]
    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    private float _timeBetweenReplacing = 2f;
    [SerializeField]
    private float _distanceToPlayer = 1f;

    private bool _isMoving;

    protected override void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _health = _startHealth;
        IsDead = false;
        _isMoving = false;

        LookOnPlayer();
        MoveAndShoot();
    }

    private void MoveAndShoot()
    {
        Helpers.Delay(() => IsDead, null, () =>
        {
            if(!_isMoving)
            {
                var newPosition = new Vector3(_player.transform.position.x - _distanceToPlayer, 0, _player.transform.position.z - _distanceToPlayer);
                if (transform.position != newPosition)
                {
                    _isMoving = true;
                    _animator.SetTrigger("Move");

                    Helpers.Delay(() => IsDead || transform.position == newPosition,
                    () =>
                    {
                        if (!IsDead)
                        {
                            _isMoving = false;
                            Shoot();
                            _player.SortEnemiesByDistance();
                        }
                    },
                    () =>
                    {
                        if (!IsDead)
                            transform.position = Vector3.Lerp(transform.position, newPosition, Time.fixedDeltaTime * _movementSpeed);
                    });
                }
            }
        }, _timeBetweenReplacing);
    }
}