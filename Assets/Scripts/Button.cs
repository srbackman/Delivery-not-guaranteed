using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private bool ButtonOn = false;

    public bool IsTriggered()
    {
        if (ButtonOn)
        {
            ButtonOn = false;
            return (true);
        }

        return (false);
    }

    public void Trigger()
    {
        ButtonOn = true;
    }
}
