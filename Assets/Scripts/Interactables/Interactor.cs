using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{

    public bool wireframeEnabled;
    public Transform interactionPoint;
    [SerializeField] private float interactionPointDistance = 0.5f;
    [SerializeField] private LayerMask interactionMask;

    private readonly Collider[] colliders = new Collider [3];
    [SerializeField] private bool itemInWay;
    Player player;
    PlayerInput playerInput;

    InputAction interactAction;
    InputAction fireAction;

    bool isTryingToInteract;

    void Awake()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["interact"];
        fireAction = playerInput.actions["fire"];
    }
    void Update()
    {
        var isTryingToInteract = interactAction.ReadValue<float>() > 0;

        var isTryingToFire = fireAction.ReadValue<float>() > 0;

        //Find everything in the ray that is part of the interactable mask
        itemInWay = Physics.Raycast(interactionPoint.position, interactionPoint.position + Vector3.forward * interactionPointDistance, interactionMask);

        if(itemInWay)
        {
            var interactable = colliders[0].GetComponent<IInteractable>();
            if(interactable != null && isTryingToInteract)
            {
                interactable.Interact(this);
            }

            if(interactable != null && isTryingToFire)
            {
                var physicsObject = colliders[0].GetComponent<PhysicsObject>();
                if(physicsObject != null)
                {
                    physicsObject.ThrowObject(interactionPoint.forward, player.throwPower);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(!wireframeEnabled)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(interactionPoint.position, interactionPoint.position + Vector3.forward * interactionPointDistance);
    }
}
