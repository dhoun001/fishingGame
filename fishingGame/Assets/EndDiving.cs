using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDiving : MonoBehaviour {

    [SerializeField]
    private bool below = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (below)
                collision.gameObject.GetComponent<Player>().HaltDivingProcess();
            else
                collision.gameObject.GetComponent<Player>().HaltBackToSurfaceProcess();
        }
    }
}
