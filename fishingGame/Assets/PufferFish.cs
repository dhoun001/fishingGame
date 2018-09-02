using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PufferFish : MonoBehaviour {

    private void Awake()
    {
        StartCoroutine(Bobbing());
        ResetPosition();
    }
    private void Update()
    {

    }

    private IEnumerator Bobbing()
    {
        while(true)
        {
            GetComponent<Rigidbody2D>().DOMoveY(transform.position.y + 1, 1.5f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(1.5f);
            GetComponent<Rigidbody2D>().DOMoveY(transform.position.y - 1, 1.5f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(1.5f);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().ResetPlayer();
        }
    }

    public void ResetPosition()
    {
        float rand_x = Random.Range(-13f, 13);
        float rand_y = Random.Range(-100f, -500f);

        transform.position = new Vector3(rand_x, rand_y, transform.position.z);
    }
}
