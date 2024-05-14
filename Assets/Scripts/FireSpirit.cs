using UnityEngine;

public class FireSpirit : MonoBehaviour
{
    public float moveSpeed = 1f; // Speed of vertical movement
    public float amplitude = 1f; // Amplitude of the vertical movement

    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position of the object
        startPosition = transform.position;

        // Ignore collisions with objects tagged as "StayInPlace"
        GameObject[] stayInPlaceObjects = GameObject.FindGameObjectsWithTag("StayInPlace");
        foreach (GameObject obj in stayInPlaceObjects)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), obj.GetComponent<Collider>(), true);
        }
    }

    void Update()
    {
        // Calculate the vertical movement based on sine function
        float verticalMovement = Mathf.Sin(Time.time * moveSpeed) * amplitude;

        // Update the position of the fire spirit
        transform.position = startPosition + Vector3.up * verticalMovement;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.scoreCount += 500;
        }
    }
}
