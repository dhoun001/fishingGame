using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishManager : MonoBehaviour {

    private float totalTime;
    private ObjectPooler fishPool;

    void Awake () {
        fishPool = gameObject.GetComponent<ObjectPooler>();
    }

    void Update () {
        totalTime -= Time.deltaTime;
        if (totalTime < 0)
        {
            createFish();
        }
    }

    void createFish () {
        totalTime = Random.Range(1.5f, 3f);
        fishPool.RetrieveCopy();
    }
}
