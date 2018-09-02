using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Player : MovableByInput
{
    public AudioSource gainFishAudio;

    [Space(10)]

    public Vector2 diveDimensions;

    public Vector2 swimmingDimensions;

    [Space(10)]

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

    public int maxFishCapacity = 15;

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

    private float gravityShiftDifference = 0.75f;

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
            spriteRenderer.flipX = !fullySubmerged;
 
        }
        else if (input_direction.x < 0)
        {
            facingDirection = Vector2.left;
            spriteRenderer.flipX = fullySubmerged;
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
            GetComponent<AudioSource>().Play();
        }


    }

    private Coroutine SwingingSpear = null;
    private IEnumerator SwingSpear()
    {
        sideSpear.gameObject.SetActive(true);

        sideSpear.transform.localPosition = new Vector3(facingDirection.x, sideSpear.transform.localPosition.y, sideSpear.transform.localPosition.z);

        yield return new WaitForSeconds(swingSpearDuration);
        sideSpear.transform.localPosition = originalSideSpearPos;
        sideSpear.gameObject.SetActive(false);
    }

    private IEnumerator DivingProcess(float velocity)
    {
        currentlyDiving = true;
        GameManager.Instance.Tip.SetActive(false);
        GameManager.Instance.BoatReference.lockInput = true;
        yield return new WaitForSeconds(cannonprimeDuration);
        GameManager.Instance.FadeAmbience(false);
        GameManager.Instance.BoatReference.cannonFire.Play();
        GameManager.Instance.BoatReference.boxCollider.enabled = false;
        bottomSpear.gameObject.SetActive(true);
        //transform.position = transform.position + (Vector3.down * 5);
        GetComponent<AirCapacity>().startAir = true;
       
        rigidBody.isKinematic = false;
        rigidBody.gravityScale = gravityShiftDifference;

        ForceInDirection(-transform.up, velocity);
    }

    private IEnumerator BackToSurfaceProcess(float velocity)
    {

        currentlyDiving = true;
        GameManager.Instance.BoatReference.boxCollider.enabled = false;
        yield return new WaitForSeconds(cannonprimeDuration);
        GetComponent<Animator>().SetInteger("state", 1);
        spriteRenderer.flipY = true;
        spriteRenderer.size = diveDimensions;
        topSpear.gameObject.SetActive(true);
        GameManager.Instance.BoatReference.lockInput = true;
        rigidBody.gravityScale = -gravityShiftDifference;
        rigidBody.isKinematic = false;

        ForceInDirection(transform.up, velocity);
    }

    public void HaltDivingProcess()
    {
        bottomSpear.gameObject.SetActive(false);
        currentlyDiving = false;
        fullySubmerged = true;
        rigidBody.gravityScale = 0f;
        GetComponent<Animator>().SetInteger("state", 0);
        spriteRenderer.size = swimmingDimensions;
    }

    public void HaltBackToSurfaceProcess()
    {
        spriteRenderer.flipY = false;
        GameManager.Instance.FadeAmbience(true);
        topSpear.gameObject.SetActive(false);
        currentlyDiving = false;
        fullySubmerged = false;
        rigidBody.gravityScale = 0f;
        GameManager.Instance.BoatReference.boxCollider.enabled = true;
        transform.DOMove(GameManager.Instance.BoatReference.playerPosition.position, 2f)
            .OnComplete(() =>
            {
                GameManager.Instance.BoatReference.lockInput = false;

                //Score fish
                GameManager.Instance.UpdateScore(currentFishValue);

                currentFishValue = 0;
                currentNumberOfFish = 0;
                UpdateCurrentFishText();

                GameManager.Instance.pufferFish.ResetPosition();
                GameManager.Instance.Tip.SetActive(true);
            }

        )
        .SetEase(Ease.InExpo);
        
    }

    public void UpdateCurrentFishText()
    {
        currentFish.text = currentNumberOfFish + " / " + maxFishCapacity + " Capacity \n $" + currentFishValue;
    }

    public bool isFullCapacity { get { return currentNumberOfFish >= maxFishCapacity; } }

    private Coroutine isShowingFishScore = null;
    public void GainFish(fishBehavior fish)
    {
        //Update carried fish
        if (!isFullCapacity)
        {
            currentNumberOfFish++;
            currentFishValue += fish._scoreValue;
            UpdateCurrentFishText();

            if (showFishScore != null)
            {
                gainScore.DOFade(0f, 0.0f);
                StopCoroutine(showFishScore);
            }
            //UI
            showFishScore = StartCoroutine(ShowFishScoreAbovePlayer(fish));
            gainFishAudio.Play();
        }

    }

    private Coroutine showFishScore;

    private IEnumerator ShowFishScoreAbovePlayer(fishBehavior fish)
    {
        int score = fish._scoreValue;
        gainScore.text = "+ $" + fish._scoreValue;
        gainScore.DOFade(0f, 0.0f);
        gainScore.DOFade(1f, 0.5f);

        yield return new WaitForSeconds(1f);

        gainScore.DOFade(0f, 0.5f);
    }

    //Reset player if dead
    public void ResetPlayer()
    {
        StartCoroutine(ResettingPlayer());
    }

    private IEnumerator ResettingPlayer()
    {
        lockInput = true;
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(3f);
        transform.position = GameManager.Instance.BoatReference.playerPosition.position;
        spriteRenderer.enabled = true;
        currentFishValue = 0;
        currentNumberOfFish = 0;
        topSpear.gameObject.SetActive(false);
        bottomSpear.gameObject.SetActive(false);
        topSpear.gameObject.SetActive(false);
        currentlyDiving = false;
        fullySubmerged = false;
        rigidBody.gravityScale = 0f;
        GameManager.Instance.BoatReference.boxCollider.enabled = true;
        lockInput = false;
        GameManager.Instance.BoatReference.lockInput = false;
        GameManager.Instance.pufferFish.ResetPosition();
        GameManager.Instance.Tip.SetActive(true);
        GetComponent<AirCapacity>().RefillAir();
        GetComponent<AirCapacity>().startAir = false;
        UpdateCurrentFishText();
        spriteRenderer.size = diveDimensions;
        spriteRenderer.flipY = false;
    }
}
