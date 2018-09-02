using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishBehavior : MonoBehaviour {
    public static int scoreModifier = 1;

    public int _scoreValue = 10;
    private int scoreValue
    {
        get { return _scoreValue * scoreModifier; }
        set { _scoreValue = value; }
    }

    private bool isRight;
    private float speed;
    private int cameraHeight;

    private Rigidbody2D fishRb;

    void Awake()
    {
        cameraHeight = 569;
        fishRb = gameObject.GetComponent<Rigidbody2D>();
        setStart();
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		if(isRight != true){
            fishRb.velocity = new Vector3(speed * -1, 0, 0);
        }
        else{
            fishRb.velocity = new Vector3(speed, 0, 0);
        }
	}

    void setStart()
    {
        if (Random.Range(0, 2) != 0)
        {
            gameObject.transform.position = new Vector3(22, Random.Range(2, cameraHeight) * -1, 80);
            isRight = false;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            gameObject.transform.position = new Vector3(-22, Random.Range(2, cameraHeight) * -1, 80);
            isRight = true;
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        speed = Random.Range(0.5f, 1.0f);
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateSoonish());
    }

    private void OnDisable()
    {
        setStart();
    }

    private IEnumerator DeactivateSoonish()
    {
        yield return new WaitForSeconds(30f);
        setStart();
        gameObject.SetActive(false);
    }

}
