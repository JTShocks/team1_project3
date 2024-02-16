using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettingsData", menuName = "ScriptableObjects/Create Player Settings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    [Range(1,10)]
    [SerializeField] private float mouseSensitivity;

    [SerializeField] private bool crosshairEnabled;

    public float MouseSensitivity => mouseSensitivity;
    public bool CrosshairEnabled => crosshairEnabled;
}
