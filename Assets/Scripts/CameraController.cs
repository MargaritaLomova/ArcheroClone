using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player;

    private Vector3 _cameraDistance;

    private void Start()
    {
        _cameraDistance = transform.position - _player.transform.position;

        _player.OnMoveAction += ChangeCameraPosition;
    }

    private void ChangeCameraPosition()
    {
        transform.position = _player.transform.position + _cameraDistance;
    }
}