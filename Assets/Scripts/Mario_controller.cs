using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Added for scene management

public class MarioController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform spawnPoint;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingRight = true;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool[] isTouchingTeleporter = new bool[2];
    private Transform currentTeleporter;

    private bool isHammerActive = false;
    private float hammerDuration = 10f;
    private float hammerEndTime;

    private int respawnCounter = 0; // Counter for respawn attempts

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }

        animator.SetBool("Motion", Mathf.Abs(moveInput) > 0);

        if (Input.GetButtonDown("Jump") && isGrounded && !isHammerActive && Time.time > hammerEndTime)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            if (animator != null)
                animator.SetTrigger("Jump");
        }

        for (int i = 0; i < isTouchingTeleporter.Length; i++)
        {
            if (isTouchingTeleporter[i] && Input.GetKeyDown(KeyCode.W) && Time.time > hammerEndTime)
            {
                TeleportToNearestTeleporter(i);
            }
        }

        animator.SetBool("Grounded", isGrounded);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("StayInPlace"))
        {
            isGrounded = true;
            animator.SetBool("Grounded", true);
        }

        if (collision.gameObject.CompareTag("Barrel"))
        {
            if (IsHammerActive())
            {
                Destroy(collision.gameObject);
            }
            else
            {
                RespawnMario(); // Call respawn function
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("StayInPlace"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Blueteleporter"))
        {
            isTouchingTeleporter[0] = true;
        }
        else if (other.CompareTag("Redteleporter"))
        {
            isTouchingTeleporter[1] = true;
        }

        if (other.CompareTag("Hammer"))
        {
            PickUpHammer();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Blueteleporter"))
        {
            isTouchingTeleporter[0] = false;
        }
        else if (other.CompareTag("Redteleporter"))
        {
            isTouchingTeleporter[1] = false;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void TeleportToNearestTeleporter(int teleporterIndex)
    {
        string teleporterTag = "";
        switch (teleporterIndex)
        {
            case 0:
                teleporterTag = "Blueteleporter";
                break;
            case 1:
                teleporterTag = "Redteleporter";
                break;
        }

        GameObject[] teleporters = GameObject.FindGameObjectsWithTag(teleporterTag);

        GameObject nearestTeleporter = null;
        float minDistance = Mathf.Infinity;
        foreach (GameObject teleporter in teleporters)
        {
            if (teleporter.transform != currentTeleporter)
            {
                float distance = Vector2.Distance(transform.position, teleporter.transform.position);
                if (distance < minDistance)
                {
                    nearestTeleporter = teleporter;
                    minDistance = distance;
                }
            }
        }

        if (nearestTeleporter != null)
        {
            transform.position = nearestTeleporter.transform.position;
            currentTeleporter = nearestTeleporter.transform;
        }
    }

    void PickUpHammer()
    {
        if (!isHammerActive)
        {
            StartCoroutine(ActivateHammerForDuration());
            hammerEndTime = Time.time + hammerDuration;
        }
    }

    IEnumerator ActivateHammerForDuration()
    {
        isHammerActive = true;
        animator.SetBool("Hammer", true);

        yield return new WaitForSeconds(hammerDuration);

        isHammerActive = false;
        animator.SetBool("Hammer", false);
    }

    public bool IsHammerActive()
    {
        if (animator != null)
        {
            return animator.GetBool("Hammer");
        }
        return false;
    }

    void RespawnMario()
    {
        // Increment respawn counter
        respawnCounter++;

        if (respawnCounter > 3)
        {
            // Reset respawn counter
            respawnCounter = 0;
            // Reset score
            ScoreManager.ResetScore();
            // Load scene 0 (or your desired scene)
            SceneManager.LoadScene(0);
        }
        else
        {
            // Respawn Mario at spawn point
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
}
