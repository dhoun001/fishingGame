using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rigidBody;

    [SerializeField]
    private float Speed = 10f;

    public bool lockInput = false;

    // Use this for initialization
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void MoveInDirection(Vector3 direction)
    {
        rigidBody.MovePosition(rigidBody.transform.position + (direction * Speed * Time.deltaTime));
    }
}
