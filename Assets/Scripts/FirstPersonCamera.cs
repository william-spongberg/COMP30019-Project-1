using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{

    [SerializeField]
    private float pitchSensitivity = 100f;
    [SerializeField]
    private float yawSensitivity = 100f;
    [SerializeField]
    private Transform playerCapsule;

    [SerializeField]
    private float rotationSmoothTime = 0.05f; // Damping time for smoothing
    private float xRotation = 0f;
    private float yRotation = 0f;


    // Start is called before the first frame update
    void Start()
    {
        // Locks cursor to the game window
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Get mouse input, adjust according to sensitivity and make it frame independent
        float mouseX = Input.GetAxis("Mouse X") * yawSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * pitchSensitivity * Time.deltaTime;

        // Subtraction used to follow natural movement of mouse
        xRotation -= mouseY;

        yRotation += mouseX;

        // Avoid flipping the camera over
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Calculate pitch (x-axis rotation)
        Quaternion pitchRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Calculate yaw (y-axis rotation)
        Quaternion yawRotation = Quaternion.Euler(0f, yRotation, 0f);

        // Apply smoothed pitch
        transform.localRotation = Quaternion.Slerp(transform.localRotation, pitchRotation, rotationSmoothTime);

        // Apply smoothed yaw
        playerCapsule.rotation = Quaternion.Slerp(playerCapsule.rotation, yawRotation, rotationSmoothTime);
    }
}
