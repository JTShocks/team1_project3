using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScanner : MonoBehaviour
{
    //This checks the object in it's area
    //If that object is the key with the correct ID, it will unlock the door
    //
    public int scannerID;

    public int keyIDToCheck;

    [SerializeField] private bool scannerActivated = false;
    public event Action<int> OnKeyScanned;

    [SerializeField] private GameObject keyToScan;
    void OnTriggerEnter(Collider collider)
    {
        if(scannerActivated)
        {
            return;
        }
        Key key = collider.GetComponent<Key>();

        if(key != null && key.keyID == keyIDToCheck)
        {
            //The key scanner should be active & send event to the listeners
            OnKeyScanned?.Invoke(scannerID);
            Debug.Log("Scanner has activated");
            scannerActivated = true;
        }
        else
        {
            Debug.Log("Scanned item is not a key");
        }
    }
}
