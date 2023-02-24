using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class BaseCharacteristicController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private TMP_Text _text;
    [SerializeField]
    private TMP_Text _changeText;

    [Space]
    [Header("Variables")]
    [SerializeField]
    private int _flyDistance = 30;
    [SerializeField]
    private float _animDuration = 2f;

    public void UpdateText(int newValue, bool isSmoothlyChange = true)
    {
        var currentValue = Int32.Parse(_text.text);
        if (newValue == currentValue)
            return;

        if (isSmoothlyChange)
            ShowChangeText(newValue, currentValue, () => SmoothlyChangeValue(newValue));
        else
            ShowChangeText(newValue, currentValue, () => _text.text = $"{newValue}");
    }

    private void SmoothlyChangeValue(int newValue)
    {
        if (newValue > Int32.Parse(_text.text))
        {
            Helpers.Delay(() => Int32.Parse(_text.text) >= newValue, () =>
            {
                _text.text = $"{newValue}";
            }, () =>
            {
                _text.text = $"{Int32.Parse(_text.text) + 1}";
            }, 0.1f);
        }
        else
        {
            Helpers.Delay(() => Int32.Parse(_text.text) <= newValue, () =>
            {
                _text.text = $"{newValue}";
            }, () =>
            {
                _text.text = $"{Int32.Parse(_text.text) - 1}";
            }, 0.1f);
        }
    }

    private void ShowChangeText(int newValue, int currentValue, Helpers.Callback callback)
    {
        var changeTextRectTransform = _changeText.rectTransform;
        float initialPos;
        float targetPos;

        if (newValue > currentValue)
        {
            var difference = newValue - currentValue;

            initialPos = changeTextRectTransform.localPosition.x + _flyDistance;
            targetPos = changeTextRectTransform.localPosition.x;

            _changeText.text = $"+ {difference}";
        }
        else
        {
            var difference = currentValue - newValue;

            initialPos = changeTextRectTransform.localPosition.x;
            targetPos = changeTextRectTransform.localPosition.x + _flyDistance;

            _changeText.text = $"- {difference}";
        }

        _changeText.DOFade(1f, _animDuration / 4).OnComplete(() =>
        {
            callback?.Invoke();
            _changeText.DOFade(0f, _animDuration);
        });

        changeTextRectTransform.DOLocalMoveX(initialPos, 0);
        changeTextRectTransform.DOLocalMoveX(targetPos, _animDuration);
    }
}