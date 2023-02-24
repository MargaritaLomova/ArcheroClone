using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MovingEnemyController : BaseEnemyController
{
    [Space]
    [Header("Moving Variables")]
    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    private float _timeBetweenReplacingAndShooting;
    [SerializeField]
    private float _distanceToPlayer = 1f;

    protected override void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _health = _startHealth;
        _isDead = false;

        LookOnPlayer();
        MoveAndShoot();
    }

    private void MoveAndShoot()
    {
        MoveToPlayer(() => Shooting());
    }

    private async void MoveToPlayer(Helpers.Callback callback)
    {
        while (!_isDead)
        {
            var newPosition = new Vector3(_player.transform.position.x - _distanceToPlayer, _player.transform.position.y, _player.transform.position.z - _distanceToPlayer);

            _animator.SetTrigger("Move");

            while (!_isDead && transform.position != newPosition)
            {
                transform.position = Vector3.Lerp(transform.position, newPosition, Time.fixedDeltaTime * _movementSpeed);

                await Task.Delay((int)(Time.fixedDeltaTime * 1000));
            }

            callback?.Invoke();
        }
    }

    protected async override void Shooting()
    {
        await Task.Delay((int)((_timeBetweenShooting + _timeBetweenReplacingAndShooting) * 1000));

        Debug.Log($"Shooting = {_isDead}");

        if (!_isDead)
            base.Shooting();
    }
}