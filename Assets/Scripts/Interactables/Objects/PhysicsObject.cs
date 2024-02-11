using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsObject : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt = "Press E to pickup";
    public string InteractionPrompt => prompt;
    Rigidbody rb;
    bool isPickedUp;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    //This is a generic script to work with ALL physics objects you can pickup and throw

    public bool Interact(Interactor interactor)
    {
        if(isPickedUp)
        {
            DropObject();
            return true;
        }

        PickupObject(interactor.interactionPoint.transform);
        return true;
    }

    void PickupObject(Transform holdPosition)
    {
        //This is the function where you pickup the items
        rb.velocity = Vector3.zero; //Stop all velocity on the object when picked up
        rb.useGravity = false;
        rb.isKinematic = true;
        transform.parent = holdPosition;
        isPickedUp = true;

    }

    void DropObject()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        transform.parent = null;
        isPickedUp = false;
    }

    public void ThrowObject(Vector3 direction, float throwForce)
    {
        //This is the function that lets the player throw the object
        rb.isKinematic = false;
        transform.parent = null;
        rb.useGravity = true;
        rb.AddForce(direction * throwForce);
    }

}