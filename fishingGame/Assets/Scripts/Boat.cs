using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boat : MovableByInput
{
    public Transform playerPosition;

    public AudioSource cannonFire;

    private new void Awake()
    {
        base.Awake();
        StartCoroutine(Bobbing());
    }

    // Update is called once per frame
    void Update()
    {
        if (lockInput)
            return;

        Vector3 movement_direction = GetInputDirection();
        movement_direction.y = 0;
        ForceInDirection(movement_direction);
        
    }

    private IEnumerator Bobbing()
    {
        while (true)
        {
            GetComponent<Rigidbody2D>().DOMoveY(transform.position.y + .5f, 3f).SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(1.5f);
            GetComponent<Rigidbody2D>().DOMoveY(transform.position.y - .5f, 3f).SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(1.5f);
        }

    }
}
