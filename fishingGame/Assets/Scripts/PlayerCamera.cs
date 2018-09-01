using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    private Transform player;

    [SerializeField]
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    public float playerY;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private Vector3 playerPosition;
    // Use this for initialization
    void Start () {
        player = GameManager.Instance.PlayerReference.gameObject.transform;
        playerPosition = player.TransformPoint(new Vector3(0, playerY, -10));
        transform.position = new Vector3(0, Mathf.Clamp(playerPosition.y, minY, maxY), -10);
    }

    void Update()
    {
        playerPosition = player.TransformPoint(new Vector3(0, playerY, -10));
        if (GameManager.Instance.PlayerReference.currentlyDiving && GameManager.Instance.PlayerReference.fullySubmerged){
            snapPlayerDown();
        }
        else if (GameManager.Instance.PlayerReference.currentlyDiving && !GameManager.Instance.PlayerReference.fullySubmerged){
            snapPlayerUp();
        }
        else
        {
            smoothPlayer();
        }
    }

    void snapPlayerUp(){
        transform.position = new Vector3(0, Mathf.Clamp(playerPosition.y - 11, minY, maxY), -10);
    }

    void snapPlayerDown()
    {
        transform.position = new Vector3(0, Mathf.Clamp(playerPosition.y + 11, minY, maxY), -10);
    }

    void smoothPlayer(){
            Vector3 goToPos = Vector3.SmoothDamp(transform.position, player.position, ref velocity, smoothTime);
            transform.position = new Vector3(0, Mathf.Clamp(goToPos.y, minY, maxY), -10);
    }
}
