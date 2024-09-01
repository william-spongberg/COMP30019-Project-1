using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed;

    public float groundFriction;

    public float modelHeight;
    public LayerMask ground;
    bool grounded; 

    public Transform orientation;

    public float jumpForce;
    public float jumpDelay;
    public float airForce;
    public KeyCode jumpKey = KeyCode.Space;

    //public float gravity;
    bool canJump = true;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //canJump = true;
    }
    
    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, modelHeight * 0.5f + 0.1f, ground);
        MoveInput();

        SpeedLimit();
        if(grounded)
        {
            rb.drag = groundFriction;
        }
        else
        {
            rb.drag = 0;
        }


    }



    private void MoveInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && canJump && grounded){
            canJump = false;

            Jump();
            
            
            Invoke(nameof(ResetJump), jumpDelay);
            
        }
    }
    
    private void FixedUpdate()
    {
        
        PlayerMove();
    }

    private void PlayerMove()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if(grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if(!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airForce, ForceMode.Force);
        }

        //if (!isGrounded) {
        //    rb.AddForce(transform.down * gravity, ForceMode.Force);

        //} else if (verticalVelocity < 0) {
        //    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //}
             
    }

    private void SpeedLimit()
    {
        Vector3 currentVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(currentVelocity.magnitude > moveSpeed)
        {
            Vector3 maxVelocity = currentVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(maxVelocity.x, rb.velocity.y, maxVelocity.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }
}