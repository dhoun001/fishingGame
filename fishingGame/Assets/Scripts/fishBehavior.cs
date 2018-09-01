using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishBehavior : MonoBehaviour {

    [SerializeField]
    public bool isRight;
    public float speed;

    private Rigidbody2D fishRb;

    void Awake()
    {
        fishRb = gameObject.GetComponent<Rigidbody2D>();
    }
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(isRight);
        Debug.Log(speed);
		if(isRight != true){
            fishRb.velocity = new Vector3(speed * -1, 0, 0);
        }
        else{
            fishRb.velocity = new Vector3(speed, 0, 0);
        }
	}


}
