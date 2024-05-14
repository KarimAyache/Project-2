using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject barrelPrefab; // Reference to the barrel prefab to spawn
    public Transform spawnPoint; // Reference to the spawn point for the barrel
    public Vector2 launchVelocity = new Vector2(5f, 0f); // Default launch velocity
    public float spawnInterval = 5f; // Time interval between barrel spawns
    public PhysicsMaterial2D noFrictionNoBounceMaterial; // Reference to the physics material with no friction and no bounce
    public string targetTag = "StayInPlace"; // Tag of the objects to stay in place

    void Start()
    {
        // Start spawning barrels on a repeating schedule
        InvokeRepeating("SpawnBarrel", 0f, spawnInterval);

        // Find all objects with the specified tag and set their Rigidbody2D to kinematic
        GameObject[] objectsToStayInPlace = GameObject.FindGameObjectsWithTag(targetTag);
        foreach (GameObject obj in objectsToStayInPlace)
        {
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }

    void SpawnBarrel()
    {
        // Check if the barrel prefab and spawn point are assigned
        if (barrelPrefab != null && spawnPoint != null)
        {
            // Instantiate a new barrel at the spawn point
            GameObject newBarrel = Instantiate(barrelPrefab, spawnPoint.position, Quaternion.identity);

            // Get the Rigidbody2D component of the spawned barrel
            Rigidbody2D rb = newBarrel.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Apply the launch velocity to the barrel
                rb.velocity = launchVelocity;
            }
            else
            {
                Debug.LogWarning("Rigidbody2D component not found on the spawned barrel.");
            }

            // Attach the DestroyOnExit script to the spawned barrel
            newBarrel.AddComponent<DestroyOnExit>();

            // Apply physics material with no friction and no bounce to the colliders of the barrel prefab
            ApplyPhysicsMaterial(newBarrel);
        }
        else
        {
            Debug.LogWarning("Barrel prefab or spawn point not assigned.");
        }
    }

    // Apply physics material with no friction and no bounce to the colliders of the barrel prefab
    void ApplyPhysicsMaterial(GameObject barrel)
    {
        Collider2D[] colliders = barrel.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.sharedMaterial = noFrictionNoBounceMaterial;
        }
    }
}
