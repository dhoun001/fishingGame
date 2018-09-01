using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fish")
        {
            fishBehavior fish = collision.gameObject.GetComponent<fishBehavior>();
            GameManager.Instance.UpdateScore(fish.value);
            fish.gameObject.SetActive(false);
        }
    }
}
