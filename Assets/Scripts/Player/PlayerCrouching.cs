using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerCrouching : MonoBehaviour
{
    [SerializeField] float crouchHeight = 1.2f;
    [SerializeField] float crouchTransitionSpeed = 10f;

    Player player;
    PlayerInput playerInput;

    InputAction crouchAction;

    //Initial holder variable for the camera default position/ offset
    Vector3 initialCameraPosition;
    float standingHeight;
    float currentHeight;

    void Awake()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        crouchAction = playerInput.actions["crouch"];
    }


    // Start is called before the first frame update
    void Start()
    {
        standingHeight = player.Height;
        initialCameraPosition = player.cameraTransform.localPosition;
    }

    void OnEnable()
    {
        player.OnBeforeMove += OnBeforeMove;
    }

    void OnDisable()
    {
        player.OnBeforeMove -= OnBeforeMove;
    }

    void OnBeforeMove()
    {
        //See if the player is trying to crouch by checking if the value changes
        var isTryingToCrouch = crouchAction.ReadValue<float>() > 0;

        //If the player is trying to crouch, set the height to the crouchHeight. Otherwise, set to standing height
        var heightTarget = isTryingToCrouch ? crouchHeight : standingHeight;

        //Create a smooth transition between the changing views
        var crouchDelta = Time.deltaTime * crouchTransitionSpeed;
        currentHeight = Mathf.Lerp(currentHeight, heightTarget, crouchDelta);

        //Calculate the diff between the current height and the standing height
        var halfHeightDifference = new Vector3(0, (standingHeight - currentHeight)/2, 0);
        var newCameraPosition = initialCameraPosition - halfHeightDifference;

        player.cameraTransform.localPosition = newCameraPosition;
        player.Height = currentHeight;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
