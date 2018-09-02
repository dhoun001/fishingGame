using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirCapacity : MonoBehaviour
{
    [SerializeField]
    private Text airText;

    [SerializeField]
    private Image fillImage;

    public int maxAir = 100;
    public float currentAir = 100;

    public bool startAir = false;

    //How much air loss per second?
    public float durationPerUnitOfAir = 0.25f;

    private void Update()
    {
        if (startAir)
        {
            currentAir -= durationPerUnitOfAir * Time.deltaTime;
            UpdateUI();

            //Player Death
            if (currentAir <= 0)
            {
                GetComponent<Player>().ResetPlayer();
            }
        }
        else
        {
            RefillAir();
        }
    }

    public void RefillAir()
    {
        currentAir = maxAir;
        UpdateUI();
    }

    public void UpdateUI()
    {
        currentAir = Mathf.Clamp(currentAir, 0f, maxAir);
        airText.text = "Air: \n" + currentAir + " / " + maxAir;
        fillImage.fillAmount = currentAir / maxAir;
    }
}
