using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {

    public void PurchaseAirTank(Purchasable p)
    {
        if (!p.canAfford)
        {
            return;
        }
        p.MakePurchase();
        p.button.interactable = false;

        GameManager.Instance.PlayerReference.GetComponent<AirCapacity>().maxAir += 50;
        GameManager.Instance.PlayerReference.GetComponent<AirCapacity>().UpdateUI();
        //Select resume button
        GameObject.Find("ShopResume").GetComponent<Button>().Select();
    }

    public void PurchasePlaqueSet(Purchasable p)
    {
        if (!p.canAfford)
        {
            return;
        }
        p.MakePurchase();
        p.button.interactable = false;

        fishBehavior.scoreModifier = 2;
        //Select resume button
        GameObject.Find("ShopResume").GetComponent<Button>().Select();
    }

    public void PurchaseFishingRod(Purchasable p)
    {
        if (!p.canAfford)
        {
            return;
        }
        p.MakePurchase();
        p.button.interactable = false;

        GameManager.Instance.PlayerReference.maxFishCapacity += 20;
        GameManager.Instance.PlayerReference.UpdateCurrentFishText();
        //Select resume button
        GameObject.Find("ShopResume").GetComponent<Button>().Select();
    }

    private void OnEnable()
    {
        GameManager.Instance.PlayerReference.lockInput = true;
        GameManager.Instance.BoatReference.lockInput = true;
    }

    private void OnDisable()
    {
        GameManager.Instance.PlayerReference.lockInput = false;
        GameManager.Instance.BoatReference.lockInput = false;
    }
}
