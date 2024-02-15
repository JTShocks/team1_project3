using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelChair : PhysicsObject
{

    [SerializeField] bool isBeingHeld;
    public override bool Interact(Interactor interactor)
    {
        AlignChair(interactor.transform.forward);
        return false;
    }

    public override void ThrowObject(Vector3 direction, float throwForce)
    {
        base.ThrowObject(direction, throwForce);
    }

    void AlignChair(Vector3 forward)
    {

    }
}
