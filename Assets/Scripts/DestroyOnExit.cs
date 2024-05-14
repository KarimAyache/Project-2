using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnExit : MonoBehaviour
{
    void Update()
    {
        // Check if the barrel has exited the screen
        if (!GetComponent<Renderer>().isVisible)
        {
            // Destroy the barrel
            Destroy(gameObject);
        }
    }
}