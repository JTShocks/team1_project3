using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{


    [SerializeField] private KeyScanner keyScanner;
    private bool requireKeyScanner;
    public bool isOpen;
    public bool isLocked;
    [SerializeField] private string prompt;

    public string InteractionPrompt => prompt;

    void Awake()
    {
        if(keyScanner == null)
        {
            //The door does not require a key to open
            requireKeyScanner = false;
        }
        else
        {
            requireKeyScanner = true;
        }

        if(requireKeyScanner)
        {
            isLocked = true;
        }
        else
        {
            isLocked = false;
        }
    }
    void OnEnable()
    {
        if(keyScanner != null)
        {
            keyScanner.OnKeyScanned += UnlockDoor;
        }
    }
    void OnDisable()
    {
        if(keyScanner != null)
        {
         keyScanner.OnKeyScanned -= UnlockDoor;
        }

    }

    public bool Interact(Interactor interactor)
    {
        if(isLocked)
        {
            Debug.Log("The door is locked.");
            return false;
        }
        if(isOpen)
        {
            CloseDoor();
            return true;
        }
        else
        {
            OpenDoor();
            return true;
        }

    }

    void OpenDoor()
    {
        //Change the state to being open
        Debug.Log("Door is now opening");
        isOpen = true;
        gameObject.SetActive(false);
    }
    void CloseDoor()
    {
        //Change the state to being closed
        Debug.Log("Door is now closing");
        isOpen = false;
    }
    void UnlockDoor(int scannerID)
    {
        if(scannerID == keyScanner.scannerID)
        {
            Debug.Log("Door has been unlocked.");
            isLocked = false;
        }
    }

}
