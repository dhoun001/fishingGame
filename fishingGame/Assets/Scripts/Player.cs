using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Player : MovableByInput
{
    [SerializeField]
    private Spear sideSpear;

    [SerializeField]
    private Spear topSpear;

    [SerializeField]
    private Spear bottomSpear;

    [SerializeField]
    private Text gainScore;

    [SerializeField]
    private Text currentFish;

    [Space(10)]

    [SerializeField]
    private int maxFishCapacity = 15;

    private int currentNumberOfFish = 0;

    private int currentFishValue = 0;

    [Space(10)]

    [SerializeField]
    private float swingSpearDuration = 1f;

    [SerializeField]
    private float lengthHitbox = 3f;

    private Vector3 originalSideSpearPos;

    [Space(10)]

    [SerializeField]
    private float divingVelocity = 30f; 
    [SerializeField]
    private float diveTurnSpeed = 2f;
    [SerializeField]
    public float cannonprimeDuration = 2f;

    private Vector2 facingDirection = Vector2.right;

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
        originalSideSpearPos = sideSpear.transform.localPosition;
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

        //Determine facing direction
        if (input_direction.x > 0)
        {
            facingDirection = Vector2.right;
            spriteRenderer.flipX = false;
        }
        else if (input_direction.x < 0)
        {
            facingDirection = Vector2.left;
            spriteRenderer.flipX = true;
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

        //Swing spear
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (SwingingSpear != null)
            {
                sideSpear.transform.localPosition = originalSideSpearPos;
                StopCoroutine(SwingingSpear);
            }

            SwingingSpear = StartCoroutine(SwingSpear());
        }


    }

    private Coroutine SwingingSpear = null;
    private IEnumerator SwingSpear()
    {
        sideSpear.gameObject.SetActive(true);

        //sideSpear.transform.DOMove(sideSpear.transform.position + (Vector3.right * lengthHitbox), swingSpearDuration);

        sideSpear.transform.localPosition = new Vector3(facingDirection.x, sideSpear.transform.localPosition.y, sideSpear.transform.localPosition.z);

        yield return new WaitForSeconds(swingSpearDuration);
        sideSpear.transform.localPosition = originalSideSpearPos;
        sideSpear.gameObject.SetActive(false);
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

                //Score fish
                GameManager.Instance.UpdateScore(currentFishValue);

                currentFishValue = 0;
                currentNumberOfFish = 0;
                currentFish.text = currentNumberOfFish + " / " + maxFishCapacity + " Capacity \n $" + currentFishValue;
            }
        );
        
    }

    public bool isFullCapacity { get { return currentNumberOfFish >= maxFishCapacity; } }

    bool showingText = false;
    bool cancel_tween = false;
    public void GainFish(fishBehavior fish)
    {
        //Update carried fish
        if (!isFullCapacity)
        {
            currentNumberOfFish++;
            currentFishValue += fish.value;
            currentFish.text = currentNumberOfFish + " / " + maxFishCapacity + " Capacity \n $" + currentFishValue;

            if (showingText)
            {
                cancel_tween = true;
            }
            showingText = true;
            //UI
            int score = fish.value;
            gainScore.text = "+ $" + fish.value;
            gainScore.DOFade(0f, 0.0f);
            gainScore.DOFade(1f, 0.5f);
            Vector2 original_loc = gainScore.transform.position;
            gainScore.transform.position = gainScore.transform.position * (Vector2.down * 5);
            Vector2 end_pos = gainScore.transform.position;
            gainScore.transform.DOMove(original_loc, 0.5f)
                .OnComplete(()=>
                {
                    if (!cancel_tween)
                    {
                        gainScore.transform.DOMove(end_pos, 0.5f);
                        gainScore.DOFade(0f, 0.5f);
                    }
                    showingText = false;
                });

        }

    }
}
