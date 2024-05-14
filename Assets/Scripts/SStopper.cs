using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SStopper : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spring"))
        {
            Rigidbody2D springRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (springRb != null)
            {
                // Stop the spring's movement
                springRb.velocity = Vector2.zero;

                // Make the spring drop straight down
                Vector2 dropDirection = Vector2.down * 10f; // Adjust the drop speed as needed
                springRb.AddForce(dropDirection, ForceMode2D.Impulse);

                // Change the physics material of the spring's colliders to one with zero bounce
                Collider2D[] colliders = collision.gameObject.GetComponentsInChildren<Collider2D>();
                foreach (Collider2D collider in colliders)
                {
                    collider.sharedMaterial.bounciness = 0f;
                }
            }
        }
        
        if (collision.gameObject.CompareTag("Player"))
        {
            // Ignore collision with the player
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, true);
        }
    }
}
