using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour, IInteractable
{
    public string InteractionPrompt => throw new System.NotImplementedException();

    //This is a generic script to work with ALL physics objects you can pickup and throw

    public bool Interact(Interactor interactor)
    {
        throw new System.NotImplementedException();
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
