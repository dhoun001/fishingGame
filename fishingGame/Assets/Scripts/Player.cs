using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MovableByInput
{
    [SerializeField]
    private Spear topSpear;

    [SerializeField]
    private Spear bottomSpear;

    [Space(10)]

    [SerializeField]
    private float divingVelocity = 30f;
    [SerializeField]
    private float diveTurnSpeed = 2f;
    [SerializeField]
    private float cannonprimeDuration = 2f;

    private bool _fullySubmerged = false;
    public bool fullySubmerged
    {
        get { return _fullySubmerged; }
        set
        {
            _fullySubmerged = value;
            rigidBody.isKinematic = !_fullySubmerged;
        }
    }

    public bool currentlyDiving = false;

    private SpriteRenderer spriteRenderer;

    protected new void Awake()
    {
        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Start()
    {
        fullySubmerged = false;
    }

    // Update is called once per frame
    void Update ()
    {
        if (lockInput)
            return;

        Vector3 input_direction = GetInputDirection();

        //Dive button
        if (Input.GetKey(KeyCode.Z) && !currentlyDiving)
        {
            if (fullySubmerged)
                StartCoroutine(BackToSurfaceProcess(divingVelocity));
            else
                StartCoroutine(DivingProcess(divingVelocity));

            return;
        }

        //Diving inputs
        if (currentlyDiving)
        {
            input_direction.y = 0;
            ForceInDirection(input_direction, diveTurnSpeed);
            return;
        }

        if (!fullySubmerged || currentlyDiving)
            return;

        //Move in all directions
        MoveInDirection(input_direction);

    }

    private IEnumerator DivingProcess(float velocity)
    {
        currentlyDiving = true;
        GameManager.Instance.BoatReference.lockInput = true;
        yield return new WaitForSeconds(cannonprimeDuration);
        bottomSpear.gameObject.SetActive(true);
        transform.position = transform.position + (Vector3.down * 2);
        rigidBody.isKinematic = false;
        rigidBody.gravityScale = 1f;

        ForceInDirection(-transform.up, velocity);
    }

    private IEnumerator BackToSurfaceProcess(float velocity)
    {
        currentlyDiving = true;
        GameManager.Instance.BoatReference.boxCollider.enabled = false;
        yield return new WaitForSeconds(cannonprimeDuration);
        topSpear.gameObject.SetActive(true);
        GameManager.Instance.BoatReference.lockInput = true;
        rigidBody.gravityScale = -1f;
        rigidBody.isKinematic = false;

        ForceInDirection(transform.up, velocity);
    }

    public void HaltDivingProcess()
    {
        bottomSpear.gameObject.SetActive(false);
        currentlyDiving = false;
        fullySubmerged = true;
        rigidBody.gravityScale = 0f;
    }

    public void HaltBackToSurfaceProcess()
    {
        topSpear.gameObject.SetActive(false);
        currentlyDiving = false;
        fullySubmerged = false;
        rigidBody.gravityScale = 0f;
        GameManager.Instance.BoatReference.boxCollider.enabled = true;
        transform.DOMove(GameManager.Instance.BoatReference.playerPosition.position, 0.75f)
            .OnComplete(() =>
            {
                GameManager.Instance.BoatReference.lockInput = false;
            }
        );
        
    }
}
