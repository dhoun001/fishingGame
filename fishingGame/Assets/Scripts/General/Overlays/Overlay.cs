using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Behavior script for Overlay ui object
/// </summary>
public class Overlay : MonoBehaviour
{
    [SerializeField]
    private Text stepNumber;

    [SerializeField]
    private Text infoText;

    [SerializeField]
    private Button closeOverlayButton;

    [SerializeField]
    private Text finishedButtonText;

    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private Button prevButton;

    private int currentStep = 0;

    private OverlayBehavior Data;

    public void Init(OverlayBehavior behavior)
    {
        if (behavior.NumberOfSteps == 0)
            return;
        Data = behavior;
        currentStep = 0;
        closeOverlayButton.gameObject.SetActive(false);
        UpdateStepButtonStatus();
        UpdateCurrentStep();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Go to next step in overlay
    /// </summary>
    public void StepNext()
    {
        currentStep++;
        UpdateCurrentStep();
        UpdateStepButtonStatus();

        if (currentStep == Data.NumberOfSteps - 1)
            closeOverlayButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Go to previous step in overlay
    /// </summary>
    public void StepPrevious()
    {
        currentStep--;
        UpdateCurrentStep();
        UpdateStepButtonStatus();

        if (currentStep == 0)
            closeOverlayButton.gameObject.SetActive(false);
    }

    private void UpdateStepButtonStatus()
    {
        if (Data.stepOrder == null)
        {
            nextButton.gameObject.SetActive(false);
            prevButton.gameObject.SetActive(false);
            return;
        }

        nextButton.gameObject.SetActive(currentStep != Data.NumberOfSteps - 1);
        prevButton.gameObject.SetActive(currentStep != 0);
    }

    /// <summary>
    /// Update step text and next highlighted rect transform
    /// </summary>
    private void UpdateCurrentStep()
    {
        stepNumber.text = currentStep + 1 + " / " + Data.NumberOfSteps;

        if (Data.stepOrder != null && currentStep < Data.NumberOfSteps)
        {
            OverlayManager.Instance.SetHighlightOnRect(Data.stepOrder[currentStep].stepHighlight);
        }
        infoText.text = Data.stepOrder[currentStep].stepInformation;
    }
}
