using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rigidBody;

    [HideInInspector]
    public BoxCollider2D boxCollider;

    [SerializeField]
    protected float Speed = 10f;

    public bool lockInput = false;

    // Use this for initialization
    protected void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void MoveInDirection(Vector3 direction, float speed = -1f)
    {
        if (direction == Vector3.zero)
            return;

        if (speed == -1f)
            speed = Speed;

        rigidBody.MovePosition(rigidBody.transform.position + (direction * Speed * Time.deltaTime));
    }

    public void ForceInDirection(Vector3 direction, float speed = -1f)
    {
        if (direction == Vector3.zero)
            return;

        if (speed == -1f)
            speed = Speed;

        rigidBody.AddForce(direction * Speed, ForceMode2D.Impulse);
    }
}
