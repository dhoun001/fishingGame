using Gamelogic.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: rename to OverlayData when external text for step info is implemetned

/// <summary>
/// Representation of Step information with optional highlighted rect transforms at each step
/// </summary>
[Serializable]
public class OverlayBehavior
{
    private string _uniqueID = "";
    public string uniqueID
    {
        get
        {
            if (_uniqueID == "")
                _uniqueID = Guid.NewGuid().ToString("N");

            return _uniqueID;
        }
    }

    public OverlayStepInspectorList stepOrder;
    public int NumberOfSteps { get { return stepOrder.Count; } }

    public enum OverlayPosition
    {
        Center,
        CenterRight,
        CenterLeft
    }

    /// <summary>
    /// Position of overlay on screen
    /// </summary>
    public OverlayPosition overlayPosition = OverlayPosition.Center;

    /// <summary>
    /// Screen position of overlay
    /// </summary>
    /// <returns></returns>
    public Vector2 GetScreenPositionOfOverlay()
    {
        switch (overlayPosition)
        {
            case OverlayPosition.CenterRight:
                return new Vector2(Screen.width / 2f + Screen.width / 4f, Screen.height / 2f);
            case OverlayPosition.CenterLeft:
                return new Vector2(Screen.width / 4f, Screen.height / 2f);
            default:
                return new Vector2(Screen.width / 2f, Screen.height / 2f);
        }
    }
}

[Serializable]
public class OverlayStep
{
    public string stepName = "Step Name";
    [TextArea]
    public string stepInformation;
    public RectTransform stepHighlight;
}

[Serializable]
public class OverlayStepInspectorList: InspectorList<OverlayStep>
{

}