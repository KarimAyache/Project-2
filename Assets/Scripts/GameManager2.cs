using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager2 : MonoBehaviour
{
    public GameObject springPrefab; // Reference to the spring prefab to spawn
    public Transform spawnPoint; // Reference to the spawn point for the spring
    public Vector2 launchVelocity = new Vector2(5f, 0f); // Default launch velocity
    public float spawnInterval = 5f; // Time interval between spring spawns
    public PhysicsMaterial2D noFrictionNoBounceMaterial; // Reference to the physics material with no friction and no bounce
    public string targetTag = "StayInPlace"; // Tag of the objects to stay in place

    void Start()
    {
        // Start spawning springs on a repeating schedule
        InvokeRepeating("SpawnSpring", 0f, spawnInterval);

        // Freeze objects with the specified tags in place
        FreezeObjectsWithTag(targetTag);
        FreezeObjectsWithTag("Freeze"); // Adding freeze tag here
    }

    void SpawnSpring()
    {
        // Check if the spring prefab and spawn point are assigned
        if (springPrefab != null && spawnPoint != null)
        {
            // Instantiate a new spring at the spawn point
            GameObject newSpring = Instantiate(springPrefab, spawnPoint.position, Quaternion.identity);

            // Get the Rigidbody2D component of the spawned spring
            Rigidbody2D rb = newSpring.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Apply the launch velocity to the spring
                rb.velocity = launchVelocity;

                // Attach the DestroyOnExit script to the spawned spring
                newSpring.AddComponent<DestroyOnExit>();

                // Apply physics material with no friction and no bounce to the colliders of the spring prefab
                ApplyPhysicsMaterial(newSpring);
            }
            else
            {
                Debug.LogWarning("Rigidbody2D component not found on the spawned spring.");
            }
        }
        else
        {
            Debug.LogWarning("Spring prefab or spawn point not assigned.");
        }
    }

    // Apply physics material with no friction and no bounce to the colliders of the spring prefab
    void ApplyPhysicsMaterial(GameObject spring)
    {
        Collider2D[] colliders = spring.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.sharedMaterial = noFrictionNoBounceMaterial;
        }
    }

    // Freeze objects with the specified tag in place
    void FreezeObjectsWithTag(string tag)
    {
        GameObject[] objectsToFreeze = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objectsToFreeze)
        {
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }
}
