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


    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;


    Vector3 velocity;
    bool isGrounded;
    bool wallRun;
    

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
		
         if(isGrounded)
         {
         	jumpCount = 0;
         }


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

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

        velocity.y += gravity * Time.deltaTime;
		controller.Move(move * speed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnControllerColliderHit (ControllerColliderHit hit){
        if(!controller.isGrounded && hit.normal.y < 0.1f)
        {
            if(Input.GetButtonDown("Jump"))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.red, 1.25f);
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                velocity.x = hit.normal * speed;
                
            }
            
        }
    }
}