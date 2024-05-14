using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTeleport : MonoBehaviour
{
    private bool canCheckTeleport = true; // Flag to control cooldown

    void Update()
    {
        if (canCheckTeleport)
        {
            CheckForTeleport();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
            if (IsTeleporter(collision.gameObject))
            {
                if (Random.Range(0, 2) == 0) // 1/2 chance of teleporting
                {
                    StartCoroutine(TeleportAndCooldown(collision.gameObject));
                }
            }
    }

    bool IsTeleporter(GameObject obj)
    {
        return obj.CompareTag("Blueteleporter") ||
               obj.CompareTag("Redteleporter") ||
               obj.CompareTag("Yellowteleporter") ||
               obj.CompareTag("Purpleteleporter");
    }

    IEnumerator TeleportAndCooldown(GameObject teleporter)
    {
        string teleporterTag = teleporter.tag;

        GameObject[] teleporters = GameObject.FindGameObjectsWithTag(teleporterTag);

        // Find the nearest teleporter
        GameObject nearestTeleporter = null;
        float minDistance = Mathf.Infinity;
        foreach (GameObject tp in teleporters)
        {
            if (tp != teleporter) // Exclude the current teleporter
            {
                float distance = Vector2.Distance(transform.position, tp.transform.position);
                if (distance < minDistance)
                {
                    nearestTeleporter = tp;
                    minDistance = distance;
                }
            }
        }

        // Teleport the barrel to the nearest teleporter
        if (nearestTeleporter != null)
        {
            transform.position = nearestTeleporter.transform.position;
            Debug.Log("Barrel teleported to: " + nearestTeleporter.name); // Debug log
        }

        // Start the cooldown coroutine
        canCheckTeleport = false; // Disable the check
        yield return new WaitForSeconds(1f); // Wait for 1 second
        canCheckTeleport = true; // Enable the check after cooldown
    }

    void CheckForTeleport()
    {
        // Check for collision with teleporters in each frame
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (Collider2D collider in colliders)
        {
            if (IsTeleporter(collider.gameObject))
            {
                return; // Exit if a teleporter is found
            }
        }
    }
}
