using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class WheelChair : PhysicsObject
{
    

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
        if(isPickedUp)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
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
        rb.isKinematic = true;
        isPickedUp = true;
    }
}
