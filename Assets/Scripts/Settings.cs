using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static string mouseSensName = "MouseSensLevel";
    public static float mouseSensLevel
    {
        get
        {
            return PlayerPrefs.GetFloat(mouseSensName, 1);
        }
        set
        {
            PlayerPrefs.SetFloat(mouseSensName, value);
        }
    }

    public static string crosshairEnabled = "crosshairEnabled";
    public static int crosshairEnabledValue
    {
        get
        {
            return PlayerPrefs.GetInt(crosshairEnabled, 1);
        }
        set
        {
            PlayerPrefs.SetInt(crosshairEnabled, value);
        }
    }
}
