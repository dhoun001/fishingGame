using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WayerSwaying : MonoBehaviour {

    [SerializeField]
    private float swayOffset = 3f;

    [SerializeField]
    private float swayDuration = 3f;

    [SerializeField]
    private Ease swayEase;

    private void Awake()
    {
        StartCoroutine(WaterSway());
    }

    private IEnumerator WaterSway()
    {
        while (true)
        {
            transform.DOMoveX(transform.position.x + swayOffset, swayDuration).SetEase(swayEase);
            yield return new WaitForSeconds(swayDuration);
            transform.DOMoveX(transform.position.x - swayOffset, swayDuration).SetEase(swayEase);
            yield return new WaitForSeconds(swayDuration);
        }
    }
}
