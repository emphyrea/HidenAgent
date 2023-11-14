using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] RectTransform thumbstick;
    [SerializeField] RectTransform background;
    [SerializeField] float deadZone = 0.2f;

    public delegate void OnInputValueChanged(Vector2 inputVal); //delegate
    public delegate void OnStickTapped(); //delegate

    public event OnInputValueChanged onInputValueChanged; //event = things outside cannot evoke it, only subscribe
    public event OnStickTapped onStickTapped;

    bool isDrag = false;
    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
        Vector3 touchPos = eventData.position;
        Vector3 thumbstickLocalOffset = Vector3.ClampMagnitude(touchPos - background.position, background.sizeDelta.x/2f);

        thumbstick.localPosition = thumbstickLocalOffset;
        Vector2 outputVal = thumbstickLocalOffset / background.sizeDelta.y * 2f;
        if (outputVal.magnitude > deadZone)
        {
             onInputValueChanged?.Invoke(outputVal); // ? = null check
        }

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
        if(isDrag)
        {
            isDrag = false;
            return;
        }
        onStickTapped?.Invoke();
    }
}
