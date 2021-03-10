using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    // clicked event
    public delegate void ButtonClick(string ID);
    public event ButtonClick OnButtonClick;
    // ID string not case sensitive casts to Upper
    [SerializeField] string buttonID = "0";

    // function called by the UI button
    public void ButtonClicked ()
    {
        // broadcast event to subscribers
        if (OnButtonClick != null)
        {
            OnButtonClick(buttonID.ToUpper());
        }
    }
}
