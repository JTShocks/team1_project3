using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsObject : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt = "Press E to pickup";
    public string InteractionPrompt => prompt;
    public Rigidbody rb;
    public bool isPickedUp;
    public bool isBeingThrown;
    [SerializeField] float stunAmount;

    Vector3 itemPlacePosition;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        //Interactor.OnThrowObject += ThrowObject;
    }
    void OnDisable()
    {
        //Interactor.OnThrowObject -= ThrowObject;
    }

    public virtual void FixedUpdate()
    {
        isBeingThrown = rb.velocity.magnitude > .5;
        if(isPickedUp)
        {
            //rb.velocity = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
                
            //transform.localPosition = Vector3.forward;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        EnemyBehaviour enemy = collision.collider.GetComponent<EnemyBehaviour>();
        if(enemy != null && isBeingThrown)
        {
            enemy.stunnedTimer = stunAmount;
            enemy.ChangeEnemyState(EnemyBehaviour.EnemyState.Stunned);

            
        }
        else
        {
           Debug.Log("Didn't hit anything"); 
        }
    }

    //This is a generic script to work with ALL physics objects you can pickup and throw

    public virtual bool Interact(Interactor interactor)
    {
        /*
        if(isPickedUp)
        {
            DropObject();
        }
        else
        {
            PickupObject(interactor.interactionPoint.transform);
        }*/
         return true;
    }

    void PickupObject(Transform holdPosition)
    {
        //This is the function where you pickup the items
        //Stop all velocity on the object when picked up
        //rb.useGravity = false;
        //rb.isKinematic = true;
        //transform.parent = holdPosition;
        itemPlacePosition = holdPosition.position;


        isPickedUp = true;

    }

    public virtual void DropObject()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        transform.parent = null;
        isPickedUp = false;
    }

    public virtual void ThrowObject(Vector3 direction, float throwForce)
    {
        if(!isBeingThrown && isPickedUp)
        {
            throwForce *= rb.mass;
            //This is the function that lets the player throw the object
            DropObject();
            rb.AddForce(direction * throwForce);

        }

    }

}
