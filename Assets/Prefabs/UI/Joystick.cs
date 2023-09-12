using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] RectTransform thumbstick;
    [SerializeField] RectTransform background;
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 touchPos = eventData.position;
        Vector3 thumbstickLocalOffset = Vector3.ClampMagnitude(touchPos - background.position, background.sizeDelta.x/2f);

        thumbstick.transform.localPosition = thumbstickLocalOffset;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("pointer down");

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        thumbstick.transform.localPosition = Vector2.zero;
    }
}
