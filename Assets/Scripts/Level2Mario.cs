using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Level2Mario : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform spawnPoint; // Set Mario's spawn point in the Inspector

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingRight = true; // Variable to track the direction the character is facing
    private Animator animator; // Reference to the Animator component
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    private bool[] isTouchingTeleporter = new bool[4]; // Flags to track if Mario is touching a teleporter of each color
    private Transform currentTeleporter; // Store the current teleporter to avoid teleporting to the same one

    private bool isHammerActive = false; // Flag to track if the hammer is active
    private float hammerDuration = 10f; // Duration of hammer activation in seconds

    private int respawnCounter = 0; // Counter for respawn attempts

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Freeze rotation on start
        animator = GetComponent<Animator>(); // Get the Animator component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component

        // Set Mario's initial position to the spawn point
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }
        else
        {
            Debug.LogWarning("Spawn point for Mario is not set.");
        }
    }

    void Update()
    {
        // Movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Flip character if moving in opposite direction
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }

        // Set animation parameter based on motion
        animator.SetBool("Motion", Mathf.Abs(moveInput) > 0);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded && !isHammerActive)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            if (animator != null)
                animator.SetTrigger("Jump"); // Trigger the jump animation if the animator is not null
        }

        // Teleportation
        for (int i = 0; i < isTouchingTeleporter.Length; i++)
        {
            if (isTouchingTeleporter[i] && Input.GetKeyDown(KeyCode.W))
            {
                if (!IsHammerActive()) // Teleport only if hammer is not active
                {
                    TeleportToNearestTeleporter(i);
                }
            }
        }

        // Update "Grounded" parameter in Animator
        if (animator != null)
            animator.SetBool("Grounded", isGrounded);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("StayInPlace") || collision.gameObject.CompareTag("Freeze"))
        {
            isGrounded = true;
            if (animator != null)
                animator.SetBool("Grounded", true); // Update the "Grounded" parameter in the Animator if the animator is not null
        }

        // Teleport back to spawn point if colliding with a Spring or Fire object
        if (collision.gameObject.CompareTag("Spring") || collision.gameObject.CompareTag("Fire"))
        {
            RespawnMario();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("StayInPlace") || collision.gameObject.CompareTag("Freeze"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if Mario is in contact with a teleporter
        if (other.CompareTag("Blueteleporter"))
        {
            isTouchingTeleporter[0] = true;
        }
        else if (other.CompareTag("Redteleporter"))
        {
            isTouchingTeleporter[1] = true;
        }
        else if (other.CompareTag("Yellowteleport"))
        {
            isTouchingTeleporter[2] = true;
        }
        else if (other.CompareTag("Purpleteleporter"))
        {
            isTouchingTeleporter[3] = true;
        }

        // Check if Mario picks up the hammer
        if (other.CompareTag("Hammer"))
        {
            PickUpHammer();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Reset the flag when Mario exits the teleporter
        if (other.CompareTag("Blueteleporter"))
        {
            isTouchingTeleporter[0] = false;
        }
        else if (other.CompareTag("Redteleporter"))
        {
            isTouchingTeleporter[1] = false;
        }
        else if (other.CompareTag("Yellowteleport"))
        {
            isTouchingTeleporter[2] = false;
        }
        else if (other.CompareTag("Purpleteleporter"))
        {
            isTouchingTeleporter[3] = false;
        }
    }

    // Function to flip the character horizontally
    void Flip()
    {
        // Switch the direction the character is facing
        facingRight = !facingRight;

        // Flip the character's scale along the x-axis
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Function to teleport Mario to the nearest teleporter of a specified color
    void TeleportToNearestTeleporter(int teleporterIndex)
    {
        // Array to store teleporter tags
        string[] teleporterTags = { "Blueteleporter", "Redteleporter", "Yellowteleporter", "Purpleteleporter" };

        // Find the tag corresponding to the teleporter color
        string teleporterTag = teleporterTags[teleporterIndex];

        // Find all objects with the specified teleporter tag
        GameObject[] teleporters = GameObject.FindGameObjectsWithTag(teleporterTag);

        // Find the nearest teleporter
        GameObject nearestTeleporter = null;
        float minDistance = Mathf.Infinity;
        foreach (GameObject teleporter in teleporters)
        {
            if (teleporter.transform != currentTeleporter) // Exclude the current teleporter
            {
                float distance = Vector2.Distance(transform.position, teleporter.transform.position);
                if (distance < minDistance)
                {
                    nearestTeleporter = teleporter;
                    minDistance = distance;
                }
            }
        }

        // Teleport Mario to the nearest teleporter
        if (nearestTeleporter != null)
        {
            transform.position = nearestTeleporter.transform.position;
            currentTeleporter = nearestTeleporter.transform;
        }
    }

    // Function to pick up the hammer and activate the "Hammer" parameter
    void PickUpHammer()
    {
        if (!isHammerActive)
        {
            StartCoroutine(ActivateHammerForDuration());
        }
    }

    // Coroutine to activate the "Hammer" parameter for a specified duration
    IEnumerator ActivateHammerForDuration()
    {
        isHammerActive = true;
        animator.SetBool("Hammer", true);

        yield return new WaitForSeconds(hammerDuration);

        isHammerActive = false;
        animator.SetBool("Hammer", false);
    }

    // Function to check if the hammer parameter is active
    bool IsHammerActive()
    {
        if (animator != null)
        {
            return animator.GetBool("Hammer");
        }
        return false; // Return false if animator is null
    }

    // Function to respawn Mario
    void RespawnMario()
    {
        // Increment the respawn counter
        respawnCounter++;

        // Check if respawn counter exceeds 3
        if (respawnCounter > 3)
        {
            // Reset respawn counter
            respawnCounter = 0;
            // Reset score (if applicable)
            ScoreManager.ResetScore();
            // Load scene 0
            SceneManager.LoadScene(0);
        }
        else
        {
            // Teleport Mario back to the spawn point
            TeleportToSpawnPoint();
        }
    }

    // Function to teleport Mario back to the spawn point
    void TeleportToSpawnPoint()
    {
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }
        else
        {
            Debug.LogWarning("Spawn point for Mario is not set.");
        }
    }
}
