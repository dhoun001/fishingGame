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

    // Use this for initialization
    void Start () {
        player = GameManager.Instance.PlayerReference.gameObject.transform;
	}

    void Update()
    {
        Vector3 playerPosition = player.TransformPoint(new Vector3(0, playerY, -10));
        transform.position = new Vector3(Mathf.Clamp(playerPosition.x, minX, maxX), Mathf.Clamp(playerPosition.y, minY, maxY), -10);
        //Vector3 goToPos = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, smoothTime);
        //transform.position = new Vector3(Mathf.Clamp(goToPos.x, minX, maxX), Mathf.Clamp(goToPos.y, minY, maxY), goToPos.z);
    }
}
