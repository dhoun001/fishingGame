using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put global references here
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Sprite WaterLevel;

    [HideInInspector]
    public Player PlayerReference;
    [HideInInspector]
    public Boat BoatReference;

    private void Awake()
    {
        PlayerReference = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        BoatReference = GameObject.FindGameObjectWithTag("Boat").GetComponent<Boat>();
    }

}
