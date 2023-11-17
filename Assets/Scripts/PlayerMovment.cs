using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera cam;

    private Vector2 velocity;
    private float inputAxis;

    public float moveSpeed = 8f;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    private void Update()
    {
        HorizontalMovement();
    }

    private void HorizontalMovement()
    {
        inputAxis = Input.GetAxis("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime);
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

}
