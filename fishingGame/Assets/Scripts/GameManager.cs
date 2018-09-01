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

    private void Awake()
    {
        PlayerReference = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

}
