using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MovableByInput
{
    // Update is called once per frame
    void Update()
    {
        if (lockInput)
            return;

        Vector3 movement_direction = GetInputDirection();
        movement_direction.y = 0;
        MoveInDirection(movement_direction);
        
    }
}
