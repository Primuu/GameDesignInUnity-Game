using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    // References
    private Rigidbody2D rb;
    private Camera cam;
    private float inputAxis;

    // Movement 
    private Vector2 velocity;
    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow((maxJumpTime / 2f), 2); // gravity is m / s^2

    // State
    public bool grounded { get; private set; }
    public bool jumping { get; private set; }

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    private void Update()
    {
        HorizontalMovement();

        grounded = rb.Raycast(Vector2.down);

        if (grounded) {
            GroundedMovement();
        }

        ApplyGravity();
    }

    private void HorizontalMovement()
    {
        inputAxis = Input.GetAxis("Horizontal");
        // Smoothly move the value from the current value to the target value
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime);

        // Resetting the value when running into walls
        if (rb.Raycast(Vector2.right * velocity.x)) {
            velocity.x = 0f;
        }

        // Setting Mario orientation 
        if (velocity.x > 0f) {
            transform.eulerAngles = Vector3.zero;
        } else if (velocity.x < 0f) {
            transform.eulerAngles = new Vector3(0f, 180f, 0f); 
        }
    }

    private void GroundedMovement() 
    {
        // Preventing gravity from increasing when the player is not falling
        velocity.y = Mathf.Max(velocity.y, 0f);

        jumping = velocity.y > 0f;

        if (Input.GetButtonDown("Jump")) {
            velocity.y = jumpForce;
            jumping = true;
        }
    }

    private void ApplyGravity()
    {
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;

        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f); // Prevent acceleration to infinity
    }

    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        position += velocity * Time.fixedDeltaTime;

        // Calculating the size of the camera window and preventing going outside it
        // Left lower corner of the screen 
        Vector2 leftEdge = cam.ScreenToWorldPoint(Vector2.zero);
        // Calculating position in the world corresponding to the upper right corner of the screen.
        Vector2 rightEdge = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        // Ensuring that player.x is between the leftEdge(+ half of Mario's size) and rightEdge(- half of Mario's size)
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        rb.MovePosition(position);
    }

    // Function responsible for the effect of "bouncing head" off a block above the player when he jumps
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp")) {
            if (transform.DotTest(collision.transform, Vector2.up)) {
                velocity.y = 0f;
            }
        }
    }

}
