using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{

    public bool isOpen;
    public bool isLocked;
    public void Interact()
    {
        if(isLocked)
        {
            return;
        }
        if(isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }

    }

    void OpenDoor()
    {
        //Change the state to being open
    }
    void CloseDoor()
    {
        //Change the state to being closed
    }
}
