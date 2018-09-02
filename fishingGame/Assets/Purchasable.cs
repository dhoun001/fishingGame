using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Purchasable : MonoBehaviour {

    [SerializeField]
    private Text costText;

    public int cost = 100;
    public Button button { get { return GetComponent<Button>(); } }

    public bool canAfford { get { return GameManager.Instance.currentScore >= cost; } }

    public void MakePurchase()
    {
        if (canAfford)
        {
            GameManager.Instance.currentScore -= cost;
        }
    }

    private void Awake()
    {
        costText.text = "$" + cost;
    }
}
