using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : Singleton<Timer> {

    public int minutes;
    public float seconds;
    public float milliseconds;

    private Text timeToDisplay;
    private float currtime;
    private float secondsMod;

    public Text endGameTimer;

    public bool pauseTime = true;

	// Use this for initialization
	void Awake () {
        currtime = 0;
        timeToDisplay = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (pauseTime)
            return;

        currtime += Time.deltaTime;
        minutes = (int)currtime / 60;
        seconds = Mathf.Round(currtime * 100f) / 100f;

        if (seconds < 10){
            timeToDisplay.text = minutes + ":" + "0" + seconds;
        }
        else{
            timeToDisplay.text = minutes + ":" + seconds;
        }
        endGameTimer.text = timeToDisplay.text;

    }

}
