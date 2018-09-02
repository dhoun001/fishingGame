using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaterSurface : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<Player>().fullySubmerged)
        {
            collision.gameObject.GetComponent<AirCapacity>().startAir = false;
            collision.gameObject.GetComponent<AirCapacity>().RefillAir();
            GetComponent<AudioSource>().Play();
            GameManager.Instance.PlayerReference.spriteRenderer.DOFade(1f, 0f);
        }
    }
}
