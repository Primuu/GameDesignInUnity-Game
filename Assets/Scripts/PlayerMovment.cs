using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector2 velocity;
    private float inputAxis;

    public float moveSpeed = 8f;

    private void Awake() 
    {
            rb = GetComponent<Rigidbody2D>();
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

        rb.MovePosition(position);
    }

}
