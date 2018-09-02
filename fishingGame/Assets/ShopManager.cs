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
        GameManager.Instance.UpdateScore();
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
        GameManager.Instance.UpdateScore();
    }

    public void PurchaseFishingRod(Purchasable p)
    {
        if (!p.canAfford)
        {
            return;
        }
        p.MakePurchase();
        p.button.interactable = false;

        GameManager.Instance.PlayerReference.maxFishCapacity += 30;
        GameManager.Instance.PlayerReference.UpdateCurrentFishText();
        //Select resume button
        GameObject.Find("ShopResume").GetComponent<Button>().Select();
        GameManager.Instance.UpdateScore();
    }

    public void PurchaseTrophy(Purchasable p)
    {
        if (!p.canAfford)
        {
            return;
        }
        p.MakePurchase();
        p.button.interactable = false;

        gameObject.SetActive(false);
        GameManager.Instance.FinishGame.SetActive(true);
        Timer.Instance.pauseTime = true;
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
