using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -12f;
    public float jumpHeight = 3f;
    public int jumpCount = 0;
    public float wallForce = 3f;

    public Transform wallCheck;
    public bool onWall;
    public float wallDistance = 0.4f;
    public LayerMask wallMask;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;



    protected Vector3 currentImpact;


    Vector3 velocity;
    bool isGrounded;
    bool wallRun;


    // Update is called once per frame
    void Update()
    {
    	Jumping();

    	WallBounce();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

       	onWall = Physics.CheckSphere(wallCheck.position, wallDistance, wallMask);

        //reset velocity and jumpcount
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            velocity.x = 0f;
            velocity.z = 0f;
            jumpCount = 0;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;


        velocity.y += gravity * Time.deltaTime;

        if (isGrounded) 
        {
        	controller.Move(move * speed * Time.deltaTime);
        	controller.Move(velocity * Time.deltaTime);
        }
        else 
        {
        	controller.Move(move * (speed / 2) * Time.deltaTime);
        	controller.Move(velocity * Time.deltaTime);
        }
		
    }
	
	//Wall bouncing
    void WallBounce() 
    {
    	if (onWall && isGrounded == false) 
    	{
    		if (Input.GetButtonDown("Jump"))
    		{
    			velocity.x = 4f;
    			controller.Move (velocity * Time.deltaTime);
    		}
    	}
    }

    //Jumping
    void Jumping()
    {
		if(Input.GetButtonDown("Jump"))
        {
            if(jumpCount == 0)
            {
            	velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            	jumpCount = 1;
            }
            else
            {
            	jumpCount = 2;
            }
        }
        else if (jumpCount == 1)
        	{
        		velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        		jumpCount = 2;
        	}
    }

    protected virtual void Move()
    {
    	Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))
    	movementInput *= speed * Time.deltaTime;
    	transform.Translate(new Vector3(movementInput.x, 0f, movementInput.y));
    }

}