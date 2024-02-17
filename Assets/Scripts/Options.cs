using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    // Start is called before the first frame update

    float mouseSensitivity;
    public void SetSensitivity(float value)
    {
        Debug.Log("New sense value = "+  value);
       Settings.mouseSensLevel = value;
    }
    public void EnableCrosshair(bool value)
    {
        if(value)
        {
        Settings.crosshairEnabledValue = 1;
        }
        else
        {
                    Settings.crosshairEnabledValue = 0;
        }

    }
}
