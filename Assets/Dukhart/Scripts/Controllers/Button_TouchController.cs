using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Button_TouchController : MonoBehaviour
{
    [SerializeField] List<UIButton> buttons = new List<UIButton>();
    // input event delegates subscibe to catch input from the button overlay
    public delegate void Button(string ID);
    public event Button OnButton;

    void OnEnable()
    {
        // add the button pressed event listener to every button in buttons
        foreach (UIButton button in buttons)
        {
            button.OnButtonClick += ButtonPressed;
        }
    }
    void OnDisable()
    {
        // remove the button pressed event listener to every button in buttons
        foreach (UIButton button in buttons)
        {
            button.OnButtonClick -= ButtonPressed;
        }
    }
    // catch when our buttons are pressed
    void ButtonPressed(string ID)
    {
        // inform subscibers a button on the controller was pressed
        Debug.Log("Button" + ID);
        if (OnButton != null) OnButton(ID);
    }
}