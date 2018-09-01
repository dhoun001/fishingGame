using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OverlayManager : Singleton<OverlayManager>
{
    [SerializeField]
    private float backgroundShade = 0.75f;

    [SerializeField]
    private float backgroundFadeDuration = 1f;

    [SerializeField]
    private float highlightExpandDuration = 1f;

    [SerializeField]
    private Ease highlightExpandEase = Ease.OutBounce;

    [Space(10)]

    [SerializeField]
    private Overlay primaryOverlay;

    /// <summary>
    /// Generally stretched over important game elements, in order to highlight its importance
    /// </summary>
    [SerializeField]
    private RectTransform highlighter;

    /// <summary>
    /// Shaded background when an overlay appearsTcharacter
    /// </summary>
    private Image overlayShade;

    private Queue<OverlayBehavior> overlayBehaviorsToDisplay = new Queue<OverlayBehavior>();

    private Coroutine currentlyDisplayingOverlay = null;

    private void Awake()
    {
        overlayShade = GetComponent<Image>();
    }

    /// <summary>
    /// Given data, show overlay
    /// </summary>
    /// <param name="data"></param>
    public void ShowOverlay(OverlayBehavior data)
    {
        overlayBehaviorsToDisplay.Enqueue(data);

        if (currentlyDisplayingOverlay == null)
            currentlyDisplayingOverlay = StartCoroutine(ShowingOverlay());
    }

    /// <summary>
    /// Iterates and displays all queued overlays
    /// </summary>
    private IEnumerator ShowingOverlay()
    {
        while(overlayBehaviorsToDisplay.Count != 0)
        {
            OverlayBehavior data = overlayBehaviorsToDisplay.Dequeue();
            PrimaryInit(data.GetScreenPositionOfOverlay());
            primaryOverlay.Init(data);
 
            yield return new WaitUntil(() => closeOverlayFlag);
            closeOverlayFlag = false;
        }
        currentlyDisplayingOverlay = null;
    }

    /// <summary>
    /// Given data, show overlay.
    /// Only works once per application per overlay data
    /// </summary>
    public void ShowOverlayOncePerApplication(OverlayBehavior data)
    {
        if (PlayerPrefs.HasKey(data.uniqueID))
            return;

        PlayerPrefs.SetInt(data.uniqueID, 1);
        ShowOverlay(data);
    }

    /// <summary>
    /// Primary initialization for all Overlays, regardless of data given
    /// </summary>
    private void PrimaryInit(Vector3 position)
    {
        overlayShade.DOFade(backgroundShade, backgroundFadeDuration);
        overlayShade.raycastTarget = true;
        primaryOverlay.transform.position = position;
    }

    /// <summary>
    /// Stretch highlight rect transform over another rect transform
    /// If target is null, hide highlight
    /// </summary>
    public void SetHighlightOnRect(RectTransform target)
    {
        if (target == null)
        {
            highlighter.gameObject.SetActive(false);
            return;
        }
        StartCoroutine(SetHighlightAtEndOfFrame(target));
    }

    private IEnumerator SetHighlightAtEndOfFrame(RectTransform target)
    {
        yield return new WaitForEndOfFrame();
        highlighter.gameObject.SetActive(true);

        highlighter.sizeDelta = Vector3.zero;
        highlighter.transform.position = target.transform.position;
        highlighter.DOSizeDelta(target.sizeDelta, highlightExpandDuration)
            .SetEase(highlightExpandEase);
    }

    public static Rect RectTransformToScreenSpace(RectTransform transform)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        float x = transform.position.x + transform.anchoredPosition.x;
        float y = Screen.height - transform.position.y - transform.anchoredPosition.y;

        return new Rect(x, y, size.x, size.y);
    }

    private bool closeOverlayFlag = false;
    /// <summary>
    /// Remove overlay from view
    /// </summary>
    public void CloseOverlay()
    {
        highlighter.gameObject.SetActive(false);
        overlayShade.DOFade(0f, backgroundFadeDuration);
        overlayShade.raycastTarget = false;
        primaryOverlay.gameObject.SetActive(false);
        closeOverlayFlag = true;
    }

   
}
