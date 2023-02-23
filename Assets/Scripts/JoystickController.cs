using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private RectTransform _joystickBackground;
    [SerializeField]
    private RectTransform _joystick;

    private Vector2 _inputPosition;

    public void OnDrag(PointerEventData eventData)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickBackground, 
                                                                   eventData.position, 
                                                                   eventData.pressEventCamera, 
                                                                   out _inputPosition))
        {
            //_inputPosition.x /= _joystickBackground.sizeDelta.x;
            //_inputPosition.y /= _joystickBackground.sizeDelta.y;

            //_inputPosition = new

            //_joystick.anchoredPosition = _inputPosition.magnitude > 1f ? _inputPosition.normalized / 4 : _inputPosition / 4;



        }
    }
}