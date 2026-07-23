using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupBubbleController : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public PopupBubbleData bubbleData;
    public Image radialFillImage;
    public Image iconImage;

    [SerializeField]
    private Image fadeIconImage;

    private float progress = 0;

    private bool mouseDown;

    #region GS
    public Image FadeIconImage { get => fadeIconImage; set => fadeIconImage = value; }
    #endregion

    private void Update()
    {
        //fill by fill% every second when held (if hold type), else drain by drain%

        if (bubbleData.type == PopupBubbleData.Type.Hold && mouseDown)
        {
            progress += bubbleData.fillSpeed * Time.deltaTime;

            if (progress > 1.0f)
            {
                Complete();
            }
        }
        else
        {
            if (progress > 0.0f)
            {
                progress -= bubbleData.drainSpeed * Time.deltaTime;
            }
        }    

        //update visual
        radialFillImage.fillAmount = progress;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //fill by click increment if click type
        if (bubbleData.type == PopupBubbleData.Type.Click)
        {
            progress += bubbleData.clickProgressIncrement;
        }

        if (progress >= 1)
        {
            Complete();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mouseDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mouseDown = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //hover effect
        return;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //remove hover effect
        return;
    }

    public void Complete()
    {
        //little pop sound would be fun

        bubbleData.onComplete?.Invoke();
        Destroy(gameObject);
    }

}

[Serializable]
public struct PopupBubbleData
{
    public enum Type
    {
        Click,
        Hold,
    }

    public Type type;

    public Sprite iconSprite;

    [Tooltip("Percent progress filled by a single click (1.0 is 100% completed)")]
    public float clickProgressIncrement;

    [Tooltip("Percent progress fill per second when held (0.0 - 1.0)")]
    public float fillSpeed;

    [Tooltip("Percent progress drain per second (0.0 - 1.0)")]
    public float drainSpeed;

    public Action onComplete;
}
