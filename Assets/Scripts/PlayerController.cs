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
    float dashTime;
    public float dashStrength;
    public float maxDashTime;
    public TrailRenderer tRender;
    public int maxJumpCount;
    public int jumpCount;
    public bool isPounding;

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
        //COYOTE TIMER && GROUND CHECK

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

        //DYNAMIC JUMP INPUTS

        if (Input.GetButtonDown("Jump") && coyoteTimer > 0)
        {
            jump = 1 * jumpStrength;
            jumpCount -= 1;

        }
        if (Input.GetButton("Jump"))
        {
            if (jump >= 0)
            {
                jump -= jumpTime * Time.deltaTime;
            }
        }
        if (Input.GetButtonUp("Jump") && jumpCount > 0)
        {
            jump = 0 * jumpStrength;
            coyoteTimer = 1;
        }
        if (Input.GetButtonUp("Jump") && jumpCount <= 0)
        {
            jump = 0 * jumpStrength;
            coyoteTimer = 0;
        }

        //DASH

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashTime = 1 * maxDashTime;
        }
        if (dashTime > 0)
        {
            dashTime -= Time.deltaTime;
        }

        //GROUNDPOUND

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            rb.gravityScale += 5;
            isPounding = true;
            IsPound();
        }
        else
        {
            isPounding = false;
        }

    }
    private void FixedUpdate()
    {
        Vector2 playerInput = new Vector2();
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), jump);
        MovementUpdate(playerInput);
        Mathf.Clamp(rb.velocity.x, - maxSpeed, maxSpeed);
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        //HORIZONTAL MOVEMENT
        Vector2 movement;
        if (BoolGrounded == true)
        {
            movement.x = playerInput.x * accelerationRate;
        }
        else
        {
            movement.x = playerInput.x * accelerationRate;
            movement.x = movement.x / airControl;
        }
        if (dashTime > maxDashTime * 0.75)
        {
            movement.x = movement.x * dashStrength;
            tRender.enabled = true;
        }
        else
        {
            tRender.enabled = false;
        }
        //VERTICAL MOVEMENT
        movement.y = playerInput.y * apexRate;
        //APPLYING TO Rigidbody
        rb.velocity += movement * Time.fixedDeltaTime;
        if (playerInput.x < 0)
        {
            currentDirection = FacingDirection.left;
        }
        if (playerInput.x > 0)
        {
            currentDirection = FacingDirection.right;
        }
    }

    public bool IsPound()
    {
        return isPounding;
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
            jumpCount = maxJumpCount;
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
        return currentDirection;
    }
}


