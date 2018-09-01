using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MovableByInput
{
    // Use this for initialization
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lockInput)
            return;

        //Dive button
        if (Input.GetKey(KeyCode.Z))
        {
            return;
        }

        Vector3 movement_direction = GetInputDirection();
        movement_direction.y = 0;
        MoveInDirection(movement_direction);
    }

    private Vector3 GetInputDirection()
    {
        Vector3 direction = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction.x = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            direction.x = 1;
        }

        return direction;
    }
}
