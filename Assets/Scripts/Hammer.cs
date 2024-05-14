using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hammer : MonoBehaviour
{
    private bool isPickingUp = false;
    private GameObject player;
    private Animator playerAnimator;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            playerAnimator = player.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                // Activate the "Hammer" parameter for the player
                playerAnimator.SetBool("Hammer", true);
            }
            // Destroy the hammer object immediately after picking it up
            Destroy(gameObject);
            ScoreManager.scoreCount += 50;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            playerAnimator = null;
        }



    }
}
