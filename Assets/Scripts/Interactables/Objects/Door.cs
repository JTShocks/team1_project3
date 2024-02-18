using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    public bool isLocked;
    [SerializeField] private KeyScanner keyScanner;
    private bool requireKeyScanner;
    public bool isOpen;
    [SerializeField] private bool isRotatingDoor;
    [SerializeField] string sceneToLoadOnExit;
    [SerializeField] private float speed = 1f;

    [Header("Rotation Configs")]
    [SerializeField] private float rotationAmount = 90f;
    [SerializeField] private float forwardDirection = 0f;
    private Vector3 startRotation;
    private Vector3 forward;
    private Coroutine animationCouroutine;

    [SerializeField] internal bool isExitDoor;


    AudioSource audioSource;
    [SerializeField] private AudioClip openDoorSound;
    [SerializeField] private AudioClip closeDoorSound;
    [SerializeField] private AudioClip lockedDoorSound;


    public delegate void ExitDoor(string levelToGoTo);
    public static ExitDoor OnExitDoorOpened;

    public string InteractionPrompt => prompt;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
            PlaySound(lockedDoorSound);
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
    public void OpenDoor(Vector3 userPosition)
    {
        PlaySound(openDoorSound);
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
        if(isExitDoor)
        {
        OnExitDoorOpened?.Invoke(sceneToLoadOnExit);
        }

    }
    void CloseDoor()
    {
        PlaySound(closeDoorSound);
        if(isOpen)
        {
            if(animationCouroutine != null)
            {
                StopCoroutine(animationCouroutine);
            }

            if(isRotatingDoor)
            {
                animationCouroutine = StartCoroutine(DoRotationClose());
            }
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(this.startRotation);

        isOpen = false;

        float time = 0;
        while(time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }
    void UnlockDoor(int scannerID)
    {
        if(scannerID == keyScanner.scannerID)
        {
            Debug.Log("Door has been unlocked.");
            isLocked = false;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
