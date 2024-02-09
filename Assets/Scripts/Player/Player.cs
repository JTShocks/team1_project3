using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // Player has the camera and moves around
    // Need to get ref to the CharacterController

    CharacterController playerController;

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
        
    }
}
