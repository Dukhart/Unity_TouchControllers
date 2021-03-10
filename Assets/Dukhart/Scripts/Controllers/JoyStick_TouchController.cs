using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class JoyStick_TouchController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public delegate void MoveJoyStick(Vector2 axis);
    public event MoveJoyStick OnMoveJoyStick;
    [SerializeField] float joystickMovementRange = 25f;
    Image jsContainer;
    Image jsStick;
    Vector2 startPosition;
    Vector2 inputAxis = Vector2.zero;
    public Vector2 InputAxis
    {
        get
        {
            return inputAxis;
        }
    }
    private void Start()
    {
        // get components
        jsContainer = GetComponent<Image>();
        jsStick = transform.GetChild(0).GetComponent<Image>();
        startPosition = jsStick.rectTransform.anchoredPosition;
    }
    public void OnDrag(PointerEventData pointerEventData)
    {
        Vector2 position = Vector2.zero;
        // get Input Direction
        RectTransformUtility.ScreenPointToLocalPointInRectangle
                (jsContainer.rectTransform,
                pointerEventData.position,
                pointerEventData.pressEventCamera,
                out position);

        // calc input axis
        position.x = (position.x / jsContainer.rectTransform.sizeDelta.x);
        position.y = (position.y / jsContainer.rectTransform.sizeDelta.y);
        float x = position.x * 2;
        float y = position.y * 2;
        inputAxis = new Vector2(x, y);
        inputAxis = (inputAxis.magnitude > 1) ? inputAxis.normalized : inputAxis;

        // Define the area in which joystick can move around
        jsStick.rectTransform.anchoredPosition = new Vector3(inputAxis.x * (jsContainer.rectTransform.sizeDelta.x + joystickMovementRange) / Mathf.PI
            ,(inputAxis.y * (jsContainer.rectTransform.sizeDelta.y + joystickMovementRange) / Mathf.PI));

        // call the joy stick moved event
        CallMoveJoyStick();
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        OnDrag(pointerEventData);
    }
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        // reset input
        inputAxis = Vector2.zero;
        // reset the joystick image to start position
        jsStick.rectTransform.anchoredPosition = startPosition;
        // call the joy stick moved event
        CallMoveJoyStick();
    }
    void CallMoveJoyStick ()
    {
        // call the joy stick moved event
        if (OnMoveJoyStick != null)
        {
            OnMoveJoyStick(inputAxis);
        }
    }
}
