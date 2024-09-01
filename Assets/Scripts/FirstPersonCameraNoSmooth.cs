
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraNoSmooth : MonoBehaviour
{

    [SerializeField]
    private float mouseSensitivity = 100f;
    [SerializeField]
    private Transform playerCapsule;

    private float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        // Locks cursor to the game window
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse input, adjust according to sensitivity and make it frame independent
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Subtraction used to follow natural movement of mouse
        xRotation -= mouseY;

        // Avoid flipping the camera over
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotates the camera up/down
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotates player left/right instead of moving the camera directly
        playerCapsule.Rotate(Vector3.up * mouseX);
        
    }
}

