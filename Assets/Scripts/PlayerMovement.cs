using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform orientation;

    [Header("Crouching & Sliding")]
    public float slideForce = 20f;
    public float slideCounterMovement = 0.2f;
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    public float boostForce = 100f;
    public bool canUncrouch;
    public bool crouching;
    public bool sliding;

    [Header("Movement")]
    public float moveSpeed = 6f;
    float movementMultiplier = 10f;
    [SerializeField] float airMultiplier = 0.4f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.C;

    [Header("Drag")]
    float groundDrag = 6f;
    float airDrag = 2f;

    [Header("Ground Detection")]
    bool isGrounded;
    float groundDistance = 0.1f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheck;

    float horizontalMovement;
    float verticalMovement;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    RaycastHit slopeHit;
    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 2 / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerScale = transform.localScale;
    }


    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();
        ControlDrag();
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
            Jump();

    slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }


    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(crouchKey) && isGrounded)
        {
            StartCrouch();
            canUncrouch = true;
        }

        if (Input.GetKeyUp(crouchKey))
        {
            if (isGrounded)
                StopCrouch();
            else if (!isGrounded && canUncrouch)
            {
                StopCrouch();
                canUncrouch = false;
            }
        }
        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }


    void StartCrouch()
    {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.velocity.magnitude > 8f)
        {
            rb.AddForce(rb.velocity.normalized * boostForce, ForceMode.Impulse);
            sliding = true;
            crouching = false;
        }  
        else
        {
            crouching = true;
            sliding = false;
        }
    }


    void StopCrouch()
    {
        crouching = false;
        sliding = false;        
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }


    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }


    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }


    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }


    private void FixedUpdate()
    {
        MovePlayer();
    }


    void MovePlayer()
    {


        if (sliding && isGrounded)
        {
            //Need to learn how to do this
            rb.AddForce(-rb.velocity.normalized * slideForce, ForceMode.Acceleration);
        }
        else if (crouching && isGrounded)
        {
            rb.AddForce(moveSpeed * -rb.velocity.normalized * slideCounterMovement * 30f);
        }

        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }
}