using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : MonoBehaviour
{

    // Player has the camera and moves around
    // Need to get ref to the CharacterController
    [Header("Player Statistics and Values")]
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float mass = 1f;
    [SerializeField] float acceleration = 20f;
    [SerializeField] float pushPower = 2.0f;
    CharacterController playerController;
    PlayerInput playerInput;
        InputAction moveAction;
        InputAction lookAction;
        InputAction sprintAction;
        
    public float Height
    {
        get => playerController.height;
        set => playerController.height = value;
    }
    public event Action OnBeforeMove;
   public Transform cameraTransform;

    Vector3 velocity;
    public float playerMoveSpeed = 8f;
    public float movementSpeedMultiplier;

    //Interactor interactor

    // What does the player need to be able to do?
    //1. Move around
    //2. Crouch (lower the camera and reduce the player hitbox)
    //3. Interact with objects (use an outsider Interactor component)
    //4. Pickup objects, then throw them in a direction

    // Start is called before the first frame update
    void Awake()
    {
        playerController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["move"];

        //interactor = GetComponent<Interactor>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGravity();
        UpdateMovement();
    }

    Vector3 GetMovementInput()
    {    
        var moveInput = moveAction.ReadValue<Vector2>();


        //Get the player input
        var input = new Vector3();
        input += transform.forward * moveInput.y;
        input += transform.right * moveInput.x;
        input = Vector3.ClampMagnitude(input, 1f);
        input *= playerMoveSpeed * movementSpeedMultiplier;
        return input;
    }

    void UpdateGravity()
    {
        var gravity = Physics.gravity * mass * Time.deltaTime;
        velocity.y = playerController.isGrounded ? - 1f : velocity.y + gravity.y;
    }

    void UpdateMovement()
    {
        movementSpeedMultiplier = 1f;
        //Check to see if anything should happen before moving
        OnBeforeMove?.Invoke();
        var input = GetMovementInput();



        //Calculate the rate the player should move each frame
        var factor = acceleration * Time.deltaTime;
        velocity.x = Mathf.Lerp(velocity.x, input.x, factor);
        velocity.z = Mathf.Lerp(velocity.z, input.z, factor);

        // var jummpInput = jumpAction.ReadValue<float>();
        //if(jumInput > 0 && controller.isGrounded)
        //{velocity.y += jumpSpeed;}

        playerController.Move(velocity * Time.deltaTime);
        
    }

    void UpdateLook()
    {

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        //If there is no rigidbody or the body is not meant to be moved
        if(body == null || body.isKinematic)
        {
            return;
        }

        //Don't push objects below us
        if(hit.moveDirection.y < -0.3f)
        {
            return;
        }

        Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.velocity = pushDirection * pushPower;
    }

}
