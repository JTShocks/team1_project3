using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Threading;
using System;
using UnityEditor;

public class Interactor : MonoBehaviour
{

    public bool wireframeEnabled;
    private bool handsAreFull;
    public Transform interactionPoint;
    [SerializeField] private float interactionPointDistance = 1;
    [SerializeField] private LayerMask interactionMask;

    [SerializeField] private TextMeshProUGUI interactText;

    private readonly Collider[] colliders = new Collider [3];
    [SerializeField] private bool itemInWay;
    float timer = 0f;
    bool isAlreadyInteracting;
    Player player;
    PlayerInput playerInput;

    InputAction interactAction;
    InputAction fireAction;

    bool isTryingToInteract;

    Rigidbody objectInHandsRB;
    GameObject heldObject;

    //public delegate void ThrowObject(Vector3 direction, float force);
    //public static ThrowObject OnThrowObject;

    void Awake()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["interact"];
        fireAction = playerInput.actions["fire"];
    }
    void Update()
    {
        timer += Time.deltaTime;
        var isTryingToInteract = interactAction.WasPressedThisFrame();

        var isTryingToFire = fireAction.ReadValue<float>() > 0;

        RaycastHit newTarget;
        if(Physics.Raycast(interactionPoint.position, interactionPoint.forward, out newTarget, interactionPointDistance, interactionMask))
        {
            IInteractable interactable = newTarget.collider.GetComponent<IInteractable>();
            if(interactable != null && heldObject == null)
            {
                ChangeInteractText(interactable.InteractionPrompt);
            }
 
        }
        else
        {
            ChangeInteractText("");
        }
        if(isTryingToInteract)
        {
            if(heldObject == null)
            {
                RaycastHit hit;
                if(Physics.Raycast(interactionPoint.position, interactionPoint.forward,out hit, interactionPointDistance, interactionMask))
                {
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                    if(interactable != null)
                    {

                        if(hit.collider.CompareTag("Item"))
                        {
                            PickupObject(hit.transform.gameObject);
                        } 

                            interactable.Interact(this);
                        
                    }
                }
            }
            else
            {
                DropObject();
            }
        }

        if(heldObject != null)
        {
            ChangeInteractText("");
        }

        if(isTryingToFire)
        {
            if(heldObject != null)
            {
                ThrowObject(interactionPoint.forward, player.throwPower);
            }
        }
        //Find everything in the ray that is part of the interactable mask
        /*if(Physics.Raycast(interactionPoint.position, interactionPoint.forward,out hit, interactionPointDistance, interactionMask))
        {
            var interactable = hit.collider.GetComponent<IInteractable>();

            if(interactable != null)
            {
                GameObject pickedObj = 
                ChangeInteractText(interactable.InteractionPrompt);
                if(isTryingToInteract)
                {
                    if(heldObject == null)
                    {
                        PickupObject(pickedObj);
                    }
                    else
                    {

                        DropObject();
                    }


                }

                
            }
            else
            {
                ChangeInteractText("");
            }

            if(heldObject != null)
            {
                interactText.text = "";
            }
        }
        else
        {
            ChangeInteractText("");
        }

        if(isTryingToFire && heldObject != null)
        {
            ThrowObject(transform.forward, player.throwPower);
        }*/
    }

    public void FixedUpdate()
    {

        if(heldObject != null)
        {
            Vector3 directionToHoldPos = (interactionPoint.position - objectInHandsRB.transform.position).normalized;
            Vector3 holdForce = directionToHoldPos * 2f;
            float distanceToHoldPos = Vector3.Distance(objectInHandsRB.transform.position, interactionPoint.position);
            distanceToHoldPos = Mathf.Clamp(distanceToHoldPos, 0.0f, 1.0f);
            holdForce *= distanceToHoldPos;

            objectInHandsRB.AddForce(holdForce, ForceMode.VelocityChange);
        }

    }

    public virtual void ThrowObject(Vector3 direction, float throwForce)
    {
        throwForce *= objectInHandsRB.mass;
        DropObject();
        objectInHandsRB.AddForce(direction* throwForce*50 * Time.fixedDeltaTime);


    }


    void PickupObject(GameObject pickObj)
    {
        if(pickObj.GetComponent<Rigidbody>())
        {
            objectInHandsRB= pickObj.GetComponent<Rigidbody>();
            objectInHandsRB.useGravity = false;
            objectInHandsRB.drag = 10;
            objectInHandsRB.constraints = RigidbodyConstraints.FreezeRotation;

            objectInHandsRB.transform.parent = interactionPoint;
            heldObject= pickObj;
        }

    }

    void DropObject()
    {
            
            objectInHandsRB.useGravity = true;
            objectInHandsRB.drag = 0;
            objectInHandsRB.constraints = RigidbodyConstraints.None;
            objectInHandsRB.transform.parent = null;
            heldObject= null;
    }

    void ChangeInteractText(string newPrompt)
    {
        interactText.text = newPrompt;
    }

    private void OnDrawGizmos()
    {
        if(!wireframeEnabled)
        {
            return;
        }
        Gizmos.color = Color.red;
    }
}
