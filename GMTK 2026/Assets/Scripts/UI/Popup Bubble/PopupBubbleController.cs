using MarUtility.UIExtensions;
using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupBubbleController : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public PopupBubbleData bubbleData;

    //COMPONENTS
    [SerializeField, BoxGroup("Components")]
    private Image radialFillImage;
    [SerializeField, BoxGroup("Components")]
    private FillController _fillCtrl;
    [SerializeField, BoxGroup("Components")]
    private Image iconImage;
    [SerializeField, BoxGroup("Components")]
    private Image fadeIconImage;
    [SerializeField, BoxGroup("Components")]
    private Animator _animator;

    //ANIMATION
    [SerializeField, BoxGroup("Animation")]
    private string _animCriticalID = "B_CRITICAL";
    [SerializeField, BoxGroup("Animation")]
    private string _animInteractID = "T_INTERACT";
    [SerializeField, BoxGroup("Animation")]
    private string _animCompleteID = "T_COMPLETED";

    private float progress = 0;
    private float timer = 0;

    private bool mouseDown;

    private bool completed;

    #region GS
    public Image FadeIconImage { get => fadeIconImage; set => fadeIconImage = value; }
    public Image IconImage { get => iconImage; set => iconImage = value; }
    #endregion


    private void Start()
    {
        StartCoroutine(Timer(0.1f));
    }

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
        //radialFillImage.fillAmount = progress;
        _fillCtrl.FillAmount = progress;
    }

    #region Input
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

        _animator.SetTrigger(_animInteractID);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mouseDown = true;
        _animator.SetTrigger(_animInteractID);
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
    #endregion

    private IEnumerator Timer(float interval)
    {
        bool timing = true;
        while (timing)
        {
            yield return new WaitForSeconds(interval);
            timer += interval;
            
            if (timer > bubbleData.GracePeriod)
            {
                _animator.SetBool(_animCriticalID, true);
                timing = false;
            }

        }
    }

    public void Complete()
    {
        if (completed) return;
        completed = true;

        //little pop sound would be fun

        bubbleData.onComplete?.Invoke();
        _animator.SetTrigger(_animCompleteID);
    }

}
//=====================================================================================================================
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

    [SerializeField, Tooltip("How long the pop-up exists before it starts draining progress.")]
    private float _gracePeriod;

    public Action onComplete;

    #region GS
    public float GracePeriod { get => _gracePeriod; set => _gracePeriod = value; }
    #endregion
}
