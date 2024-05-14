using UnityEngine;

public class BarrelPhysics : MonoBehaviour
{
    public float rollSpeed = 5f; // Speed at which the barrel rolls

    private bool isRolling = false;
    private Vector3 currentVelocity;

    void Start()
    {
        currentVelocity = Vector3.right * rollSpeed;
    }

    void Update()
    {
        if (isRolling)
        {
            // Move the barrel with its current velocity
            transform.position += currentVelocity * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("StayInPlace"))
        {
            isRolling = true;
        }
        else if (collision.gameObject.CompareTag("Border"))
        {
            // Reflect the velocity of the barrel
            ReflectVelocity(collision.contacts[0].normal);
        }

        // Check if the collision is with Mario
        if (collision.gameObject.CompareTag("Player"))
        {
            MarioController marioController = collision.gameObject.GetComponent<MarioController>();
            if (marioController != null)
            {
                if (marioController.IsHammerActive())
                {
                    // Destroy the barrel and increase the score by 100 if Mario is in hammer mode
                    Destroy(gameObject);
                    ScoreManager.scoreCount += 100;
                }
                // If Mario is not in hammer mode, do nothing
            }
        }
    }

    // Reflects the velocity of the barrel based on the collision normal
    void ReflectVelocity(Vector2 collisionNormal)
    {
        // Calculate the reflected velocity using the reflection formula
        currentVelocity = Vector2.Reflect(currentVelocity, collisionNormal);
    }
}
