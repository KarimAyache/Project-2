using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDownMovement : MonoBehaviour
{
    public float speed = 2.0f; // Speed of movement
    public float distance = 2.0f; // Distance to move up and down
    private Vector3 startPosition;
    private bool movingUp = true;

    void Start()
    {
        // Record the initial position
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the next position based on whether the object is moving up or down
        Vector3 nextPosition = transform.position;
        if (movingUp)
        {
            nextPosition.y += speed * Time.deltaTime;
        }
        else
        {
            nextPosition.y -= speed * Time.deltaTime;
        }

        // Check if the object has reached its upper or lower limit, and change direction if so
        if (nextPosition.y >= startPosition.y + distance)
        {
            movingUp = false;
        }
        else if (nextPosition.y <= startPosition.y - distance)
        {
            movingUp = true;
        }

        // Move the object to the next position
        transform.position = nextPosition;
    }
}