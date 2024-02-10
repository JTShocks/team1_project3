using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    // Player has the camera and moves around
    // Need to get ref to the CharacterController

    CharacterController playerController;

    Vector3 movementDirection;
    Vector2 inputDirection;
    public float playerMoveSpeed = 8f;

    // What does the player need to be able to do?
    //1. Move around
    //2. Crouch (lower the camera and reduce the player hitbox)
    //3. Interact with objects (use an outsider Interactor component)
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        movementDirection = (transform.right * inputDirection.x + transform.forward * inputDirection.y);
        playerController.Move(movementDirection * playerMoveSpeed * Time.deltaTime);
    }

    void OnMove(InputValue value)
    {
        inputDirection = value.Get<Vector2>();
        
    }
}
