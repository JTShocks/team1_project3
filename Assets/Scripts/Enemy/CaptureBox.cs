using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureBox : MonoBehaviour
{
    public delegate void PlayerCaught();
    public static PlayerCaught OnPlayerCaught;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            OnPlayerCaught?.Invoke();
        }
    }
}
