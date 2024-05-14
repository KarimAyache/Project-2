using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float bounceForce = 10f; // Force applied to the object when bouncing
    public float horizontalSpeed = 5f; // Horizontal speed of the object after bouncing
    public float verticalSpeed = 5f; // Vertical speed of the object after bouncing
    public float bounceDistance = 1f; // Distance the object will bounce upwards

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found.");
        }

        // Freeze rotation constraints
        rb.freezeRotation = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with an object tagged as "SStopper"
        if (collision.gameObject.CompareTag("SStopper"))
        {
            return; // Do nothing if colliding with an object tagged as "SStopper"
        }

        // Check if the collision is with an object tagged as "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Destroy the spring object
            Destroy(gameObject);
            return;
        }

        // Calculate the bounce direction based on the collision normal
        Vector2 bounceDirection = Vector2.Reflect(rb.velocity.normalized, collision.contacts[0].normal);

        // Apply the bounce force to the object
        rb.velocity = bounceDirection * bounceForce;

        // Optionally, move the object upwards by a certain distance after bouncing
        Vector2 newPosition = transform.position + new Vector3(0, bounceDistance, 0);
        rb.MovePosition(newPosition);

        // Adjust the velocity to control the speed of bouncing back up
        rb.velocity = new Vector2(rb.velocity.x * horizontalSpeed, Mathf.Abs(rb.velocity.y) * verticalSpeed);

        if (collision.gameObject.CompareTag("Freeze"))
        {
            // Ignore collision with the player
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, true);
        }
    }
}
