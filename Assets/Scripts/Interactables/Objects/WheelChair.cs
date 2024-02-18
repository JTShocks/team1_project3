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

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, whatIsGround))
        {
            groundPosition = hit.point;
        }

        if(isPickedUp)
        {
            Vector3 directionToHoldPos = (holdPosition- rb.transform.position).normalized;
            Vector3 holdForce = directionToHoldPos * .5f;
            float distanceToHoldPos = Vector3.Distance(rb.transform.position, holdPosition);
            distanceToHoldPos = Mathf.Clamp(distanceToHoldPos, 0.0f, 1.0f);
            holdForce *= distanceToHoldPos;

            rb.AddForce(holdForce, ForceMode.VelocityChange);
            //rb.velocity = Vector3.zero;
            //transform.position = new Vector3(transform.position.x,groundPosition.y,transform.position.z);
        }
    }

    public override void ThrowObject(Vector3 direction, float force)
    {
        base.ThrowObject(direction, force);
    }


    void AlignChair(Transform interactor)
    {
        holdPosition = new Vector3(interactor.position.x, groundPosition.y, interactor.position.z);
        transform.parent = interactor;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        //rb.isKinematic = true;
        isPickedUp = true;
    }
}
