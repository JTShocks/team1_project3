using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : MonoBehaviour
{

    // Player has the camera and moves around
    // Need to get ref to the CharacterController
    [Header("Player Statistics and Values")]
    [SerializeField] float mass = 1f;
    [SerializeField] float acceleration = 20f;
    [SerializeField] float pushPower = 2.0f;
    public float throwPower = 10f;
    CharacterController playerController;


    public bool isGrounded => playerController.isGrounded;
    PlayerInput playerInput;
        InputAction moveAction;
        InputAction lookAction;
        InputAction sprintAction;
        InputAction jumpAction;
        InputAction interactAction;
        
    public float Height
    {
        get => playerController.height;
        set => playerController.height = value;
    }
    public event Action OnBeforeMove;
   public Transform cameraTransform;

    internal Vector3 velocity;
    public float playerMoveSpeed = 8f;
    public float movementSpeedMultiplier;

    internal AudioSource footstepAudio;

    [SerializeField] GameObject crosshair;

    //When the player makes it to the exit, the game should fade to white, then reset back to the main menu

    // Main Menu > Level >if Game over < Reset level : else > do a white out and return to main menu

    // NICE TO HAVE OPTIONS
    // Settings menu to change sensitivity or remove the crosshair
    //  Gives something a little bit extra for the teacher when playing and should be easy to implement
    void Awake()
    {
        playerController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["move"];
        footstepAudio = GetComponent<AudioSource>();

        crosshair.SetActive(PlayerPrefs.GetInt("crosshairEnabled") > 0); 
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        UpdateGravity();
        UpdateMovement();


        var moveInput = moveAction.ReadValue<Vector2>();
        if(moveInput.magnitude > 0 && playerController.isGrounded)
        {
            footstepAudio.enabled = true;
        }
        else
        {
            footstepAudio.enabled = false;
        }


    }

    Vector3 GetMovementInput()
    {    
        var moveInput = moveAction.ReadValue<Vector2>();
        //If the player is moving, enable the footsteps

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
