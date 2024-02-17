using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    [SerializeField] float minViewDistance = 90f;
    [SerializeField] Transform playerBody;

    public float mouseSensitivity => PlayerPrefs.GetFloat("MouseSensLevel", 1);

    float xRotation = 0f;

    void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Get the input from the mouse
        float mouseX = Input.GetAxis("Mouse X") * (mouseSensitivity*100) *Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * (mouseSensitivity*100) *Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, minViewDistance);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
