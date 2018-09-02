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
    public AudioSource lakeAmbience;

    [Space(10)]

    public GameObject Tip;

    public PufferFish pufferFish;

    [SerializeField]
    private GameObject shopMenu;

    [SerializeField]
    private Text scoreTracker;

    [SerializeField]
    private GameObject menu;

    public int currentScore = 0;

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
            if (shopMenu.activeInHierarchy)
            {
                shopMenu.gameObject.SetActive(false);
                return;
            }

            menu.gameObject.SetActive(!menu.gameObject.activeInHierarchy);
            SetGameStatus(!menu.gameObject.activeInHierarchy);
            if (menu.gameObject.activeInHierarchy)
            {
                Button select = menu.transform.GetChild(0).GetComponent<Button>();
                if (select != null)
                    select.Select();
            }
            return;
        }

        //Open shopmenu
        if (Input.GetKeyDown(KeyCode.S) && !PlayerReference.currentlyDiving && !PlayerReference.fullySubmerged)
        {
            if (shopMenu.activeInHierarchy)
            {
                shopMenu.gameObject.SetActive(false);
                return;
            }

            shopMenu.gameObject.SetActive(true);
            //Select resume button
            GameObject.Find("ShopResume").GetComponent<Button>().Select();
        }
    }

    public void UpdateScore(int new_points)
    {
        currentScore += new_points;
        scoreTracker.text = "$" + currentScore.ToString();
    }

    public void UpdateScore()
    {
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
