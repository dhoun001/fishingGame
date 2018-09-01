using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Centers the gameobject either in the center of the screen, or the center of its parent on Awake
/// </summary>
public class CenterOnAwake : MonoBehaviour
{
    private void Awake()
    {
        if (transform.parent == null)
            transform.position = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        else
            transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y, 0);
    }
}
