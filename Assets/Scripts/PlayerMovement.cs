using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // References
    private Rigidbody2D rb;
    private Camera cam;
    private Collider2D col;
    private Player player;
    private float inputAxis;

    public AudioClip jumpAudioSmall;
    public AudioClip jumpAudioBig;

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
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        cam = Camera.main;
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        rb.isKinematic = false;
        col.enabled = true;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void OnDisable()
    {
        rb.isKinematic = true;
        col.enabled = false;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void Update()
    {
        if (MenuManager.Instance.isMenuActive) return;

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

            if (player.small) {
                AudioManager.Instance.PlaySFX(jumpAudioSmall);
            } else if (player.big) {
                AudioManager.Instance.PlaySFX(jumpAudioBig);
            }
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

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            // Logic responsible for the effect of "enemy bouncing"
            if (transform.DotTest(collision.transform, Vector2.down)) {
                velocity.y = jumpForce / 2f;
                jumping = true;
            }
        } else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp")) {
            // Logic responsible for the effect of "bouncing head" off a block above the player when he jumps
            if (transform.DotTest(collision.transform, Vector2.up)) {
                velocity.y = 0f;
            }
        }
    }

}
