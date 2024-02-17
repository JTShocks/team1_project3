using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class WheelChair : PhysicsObject
{
    Vector3 groundPosition;
    Vector3 holdPosition;
    [SerializeField] LayerMask whatIsGround;

    public override bool Interact(Interactor interactor)
    {
        if(!isPickedUp)
        {
            AlignChair(interactor.transform);

        }
        else
        {
            base.DropObject();
        }

        return true;
    }

    public override void Update()
    {
        base.Update();
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, whatIsGround))
        {
            groundPosition = hit.point;
        }

        if(isPickedUp)
        {
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x,groundPosition.y,transform.position.z);
        }
    }

    public override void ThrowObject(Vector3 direction, float force)
    {
        base.ThrowObject(direction, force);
    }


    void AlignChair(Transform interactor)
    {
        transform.parent = interactor;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        //rb.isKinematic = true;
        isPickedUp = true;
    }
}
