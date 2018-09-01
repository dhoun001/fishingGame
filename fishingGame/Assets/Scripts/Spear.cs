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
            fish.gameObject.SetActive(false);
            GameManager.Instance.PlayerReference.GainFish(fish);
        }
    }
}
