using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour, IInteractable
{

    //This is a generic script to work with ALL physics objects you can pickup and throw
    public void Interact()
    {
        Pickup();
    }

    void Pickup()
    {
        //This is the function where you pickup the items
    }

    void ThrowObject()
    {
        //This is the function that lets the player throw the object
    }

}
