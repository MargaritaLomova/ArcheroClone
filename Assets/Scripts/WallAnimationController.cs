using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class WallAnimationController : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _meshRenderer;
    [SerializeField]
    private float _maxOffset = 5f;
    [SerializeField]
    private float _animTime = 0.1f;

    private Material _material;
    private bool _isNeedToPlus;
    private bool _isDead;

    private void Start()
    {
        _material = _meshRenderer.materials[0];
        _isNeedToPlus = true;
        _isDead = false;

        Helpers.Delay(() => _isDead, null, () =>
        {
            if (!_isDead)
            {
                var offsetX = _material.mainTextureOffset.x;
                var offsetY = _material.mainTextureOffset.y;

                if (_isNeedToPlus && offsetY < _maxOffset)
                {
                    _isNeedToPlus = offsetY + Time.fixedDeltaTime < _maxOffset;
                    _material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY + Time.fixedDeltaTime));
                }
                else if (!_isNeedToPlus && offsetY > -_maxOffset)
                {
                    _isNeedToPlus = offsetY - Time.fixedDeltaTime <= -_maxOffset;
                    _material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY - Time.fixedDeltaTime));
                }
            }
        }, _animTime);
    }

    private void OnDestroy()
    {
        _isDead = true;
    }
}