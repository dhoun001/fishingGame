using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Put global references here
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Text scoreTracker;

    [SerializeField]
    private GameObject menu;

    private int currentScore = 0;

    [Space(10)]

    [HideInInspector]
    public Player PlayerReference;
    [HideInInspector]
    public Boat BoatReference;

    private void Awake()
    {
        PlayerReference = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        BoatReference = GameObject.FindGameObjectWithTag("Boat").GetComponent<Boat>();
    }

    private void Start()
    {
        UpdateScore(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.gameObject.SetActive(!menu.gameObject.activeInHierarchy);
            SetGameStatus(!menu.gameObject.activeInHierarchy);
            if (menu.gameObject.activeInHierarchy)
            {
                Button select = menu.transform.GetChild(0).GetComponent<Button>();
                if (select != null)
                    select.Select();
            }
        }
    }

    public void UpdateScore(int new_points)
    {
        currentScore += new_points;
        scoreTracker.text = "$" + currentScore.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetGameStatus(bool status)
    {
        if (status)
        {
            Time.timeScale = 1;
            if (PlayerReference.currentlyDiving)
            {
                PlayerReference.lockInput = false;
            }
            else
            {
                BoatReference.lockInput = false;
            }
        }
        else
        {

            Time.timeScale = 0;
            if (PlayerReference.currentlyDiving)
            {
                PlayerReference.lockInput = true;
            }
            else
            {
                BoatReference.lockInput = true;
            }

        }



    }
}
