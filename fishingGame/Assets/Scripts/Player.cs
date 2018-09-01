using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovableByInput
{
    [SerializeField]
    private float divingVelocity = 30f;

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

    private Coroutine currentlyDiving = null;

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
        if (Input.GetKey(KeyCode.Z) && currentlyDiving == null)
        {
            if (fullySubmerged)
                currentlyDiving = StartCoroutine(BackToSurfaceProcess(divingVelocity));
            else
                currentlyDiving = StartCoroutine(DivingProcess(divingVelocity));

            return;
        }

        //Diving inputs
        if (currentlyDiving != null)
        {
            input_direction.y = 0;
            ForceInDirection(input_direction, Speed / 2f);
            return;
        }

        if (!fullySubmerged || currentlyDiving != null)
            return;

        //Diving
        MoveInDirection(input_direction);

    }

    private IEnumerator DivingProcess(float velocity)
    {
        GameManager.Instance.BoatReference.lockInput = true;
        yield return new WaitForSeconds(cannonprimeDuration);
        transform.position = -transform.up * 1;
        rigidBody.isKinematic = false;
        rigidBody.gravityScale = 1f;

        ForceInDirection(-transform.up, velocity);
    }

    private IEnumerator BackToSurfaceProcess(float velocity)
    {
        GameManager.Instance.BoatReference.lockInput = true;
        rigidBody.gravityScale = -1f;
        rigidBody.isKinematic = false;

        ForceInDirection(transform.up, velocity);

        HaltDivingProcess();
        rigidBody.gravityScale = 1f;
    }

    public void HaltDivingProcess()
    {
        currentlyDiving = null;
        fullySubmerged = true;
        rigidBody.gravityScale = 0f;
    }
}
