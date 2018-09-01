using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovableByInput
{
    public bool isDiving = false;

	// Update is called once per frame
	void Update ()
    {
        if (lockInput)
            return;

        //Dive button
        if (Input.GetKey(KeyCode.Z))
        {
            return;
        }

        //Move diver or boat
        if (isDiving)
        {
            Vector3 movement_direction = GetInputDirection();
            MoveInDirection(movement_direction);
        }

    }
}
