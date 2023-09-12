using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] RectTransform thumbstick;
    [SerializeField] RectTransform background;

    public delegate void OnInputValueChanged(Vector2 inputVal); //delegate
    public event OnInputValueChanged onInputValueChanged; //event = things outside cannot evoke it, only subscribe

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 touchPos = eventData.position;
        Vector3 thumbstickLocalOffset = Vector3.ClampMagnitude(touchPos - background.position, background.sizeDelta.x/2f);

        thumbstick.localPosition = thumbstickLocalOffset;
        onInputValueChanged?.Invoke(thumbstickLocalOffset/background.sizeDelta.y * 2f); // ? = null check
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        background.position = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        background.localPosition = Vector2.zero;
        thumbstick.localPosition = Vector2.zero;
        onInputValueChanged?.Invoke(Vector2.zero);

    }
}
