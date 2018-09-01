using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishBehavior : MonoBehaviour {

    private bool isRight;
    private float speed;
    private int cameraHeight;

    private Rigidbody2D fishRb;

    void Awake()
    {
        cameraHeight = 9;
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
            gameObject.transform.position = new Vector3(7, Random.Range(0, cameraHeight) - 4, 80);
            isRight = false;
        }
        else
        {
            gameObject.transform.position = new Vector3(-7, Random.Range(0, cameraHeight) - 4, 80);
            isRight = true;
        }
        speed = Random.Range(0.5f, 1.0f);
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateSoonish());
    }

    private IEnumerator DeactivateSoonish()
    {
        yield return new WaitForSeconds(30f);
        setStart();
        gameObject.SetActive(false);
    }

}
