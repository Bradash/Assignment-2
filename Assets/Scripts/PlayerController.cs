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

    public enum FacingDirection
    {
        left, right
    }
    private FacingDirection currentDirection;

    private void Start()
    {
        accelerationRate = maxSpeed / accelerationTime;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = new Vector2();
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        MovementUpdate(playerInput);
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        rb.velocity += playerInput * accelerationRate * Time.deltaTime;
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
            return false;
        }
        else
        {
            return true;
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


