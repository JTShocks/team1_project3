using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Threading;

public class Interactor : MonoBehaviour
{

    public bool wireframeEnabled;
    private bool handsAreFull;
    public Transform interactionPoint;
    [SerializeField] private float interactionPointDistance = 0.5f;
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

        RaycastHit hit;
        //Find everything in the ray that is part of the interactable mask
        if(Physics.Raycast(interactionPoint.position, interactionPoint.forward,out hit, interactionPointDistance, interactionMask))
        {
            var interactable = hit.collider.GetComponent<IInteractable>();

            var physicsObject = hit.collider.GetComponent<PhysicsObject>();
            //handsAreFull = physicsObject.isPickedUp;
            if(interactable != null)
            {
                ChangeInteractText(interactable.InteractionPrompt);
                if(isTryingToInteract)
                {
                    interactable.Interact(this);


                }

                if(isTryingToFire)
                {
                    if(physicsObject != null)
                    {
                        physicsObject.ThrowObject(interactionPoint.forward, player.throwPower);
                    }
                }
            }
            else
            {
                ChangeInteractText("");
            }

            if(handsAreFull)
            {
                interactText.text = "";
            }
        }
        else
        {
            ChangeInteractText("");
        }
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
