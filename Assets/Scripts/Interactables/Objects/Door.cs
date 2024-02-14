using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{


    [SerializeField] private KeyScanner keyScanner;
    private bool requireKeyScanner;
    public bool isOpen;
    [SerializeField] private bool isRotatingDoor;
    [SerializeField] private float speed = 1f;
    public bool isLocked;
    [SerializeField] private string prompt;

    [Header("Rotation Configs")]
    [SerializeField] private float rotationAmount = 90f;
    [SerializeField] private float forwardDirection = 0f;
    private Vector3 startRotation;
    private Vector3 forward;
    private Coroutine animationCouroutine;


    public string InteractionPrompt => prompt;

    void Awake()
    {
        startRotation = transform.rotation.eulerAngles;
        forward = transform.right;
        if(keyScanner == null)
        {
            //The door does not require a key to open
            requireKeyScanner = false;
        }
        else
        {
            requireKeyScanner = true;
        }

        if(requireKeyScanner)
        {
            isLocked = true;
        }
        else
        {
            isLocked = false;
        }
    }
    void OnEnable()
    {
        if(keyScanner != null)
        {
            keyScanner.OnKeyScanned += UnlockDoor;
        }
    }
    void OnDisable()
    {
        if(keyScanner != null)
        {
         keyScanner.OnKeyScanned -= UnlockDoor;
        }

    }

    public bool Interact(Interactor interactor)
    {
        if(isLocked)
        {
            Debug.Log("The door is locked.");
            return false;
        }
        if(isOpen)
        {
            CloseDoor();
            return true;
        }
        else
        {
            OpenDoor(interactor.transform.position);
            return true;
        }

    }

    private IEnumerator DoRotationOpen(float forwardAmount)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if(forwardAmount >= forwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, startRotation.y - rotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, startRotation.y + rotationAmount, 0));
        }

        isOpen = true;
        float time =0;
        while(time <1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }

    void OpenDoor(Vector3 userPosition)
    {
        if(!isOpen)
        {
            if(animationCouroutine != null)
            {
                StopCoroutine(animationCouroutine);
            }
        }
        if(isRotatingDoor)
        {
            float dot = Vector3.Dot(forward, (userPosition - transform.position).normalized);
            animationCouroutine = StartCoroutine(DoRotationOpen(dot));
        }
        //Change the state to being open
        Debug.Log("Door is now opening");
        isOpen = true;
        gameObject.SetActive(false);
    }
    void CloseDoor()
    {
        //Change the state to being closed
        Debug.Log("Door is now closing");
        isOpen = false;
    }
    void UnlockDoor(int scannerID)
    {
        if(scannerID == keyScanner.scannerID)
        {
            Debug.Log("Door has been unlocked.");
            isLocked = false;
        }
    }

}
