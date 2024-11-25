using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public FacingDirection direction;
    public bool BoolGrounded;
    public float maxSpeed;
    public float accelerationTime;
    private float accelerationRate;
    public float jumpStrength;
    public float jump;
    public float jumpTime;
    public float apexHeight;
    public float apexTime;
    private float apexRate;
    public float airControl;
    public float terminalSpeed;
    public float terminalTime;
    public float coyoteTime;
    public float coyoteTimer;

    public enum FacingDirection
    {
        left, right
    }
    private FacingDirection currentDirection;

    private void Start()
    {
        accelerationRate = maxSpeed / accelerationTime;
        apexRate = apexHeight / apexTime;
    }
    private void Update()
    { 

        if (BoolGrounded == false && coyoteTimer > 0)
        {
            coyoteTimer -= Time.deltaTime;
        }
        if (coyoteTimer <= 0 & BoolGrounded == false)
        {
            rb.gravityScale += terminalSpeed / terminalTime * Time.deltaTime;
        }
            if (BoolGrounded == true)
        {
            rb.gravityScale = 1;
            coyoteTimer = coyoteTime;
        }
        if (Input.GetButtonDown("Jump") && coyoteTimer > 0)
        {
            jump = 1 * jumpStrength;
        }
        if (Input.GetButton("Jump"))
        {
            jump -= jumpTime * Time.deltaTime;
        }
        if (Input.GetButtonUp("Jump"))
        {
            jump = 0 * jumpStrength;
            coyoteTimer = 0;
        }
        
    }
    private void FixedUpdate()
    {
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = new Vector2();
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), jump);
        MovementUpdate(playerInput);
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        Vector2 movement;
        if (BoolGrounded == true)
        {
            movement.x = playerInput.x * accelerationRate;
        }
        else
        {
            movement.x = playerInput.x * accelerationRate / airControl;
        }
        
        movement.y = playerInput.y * apexRate;
        rb.velocity += movement * Time.deltaTime;
    }

    public bool IsWalking()
    {
        if (rb.velocity.x != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)   
    {
        //Check to see if the Collider's name is "Chest"
        if (collision.collider.tag == "Ground")
        {
            BoolGrounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        BoolGrounded = false;
    }



    public bool IsGrounded()
    {
        if (BoolGrounded == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
        

    public FacingDirection GetFacingDirection()
    {
        if (rb.velocity.x < 0)
        {
            currentDirection = FacingDirection.left;
        }
        if (rb.velocity.x > 0)
        {
            currentDirection = FacingDirection.right;
        }
        return currentDirection;
    }
}


