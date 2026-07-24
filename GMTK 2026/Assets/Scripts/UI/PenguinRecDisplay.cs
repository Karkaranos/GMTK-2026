using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PenguinRecDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private RecReferences[] recReferences;

    private InputAction clickAction;
    private bool isMouseOver;

    #region Nested
    [System.Serializable]
    private struct RecReferences
    {
        [SerializeField] internal RocketSection section;
        [SerializeField] internal Image image;
    }
    #endregion

    private void Awake()
    {
        clickAction = InputSystem.actions.FindAction("Click");
    }

    private void OnDestroy()
    {
        clickAction.started -= HandleClick;
    }

    public void ShowPenguin(Penguin penguin)
    {
        TogglePopup(true);
        // Show the penguin's information.
        foreach(var rec in recReferences)
        {
            RocketPart part = penguin.Recommendations[rec.section];
            rec.image.sprite = part.Sprite;
        }
    }

    public void HidePopup()
    {
        TogglePopup(false);
    }

    public void TogglePopup(bool shown)
    {
        SceneSwitcher.CanSwitchScenes = !shown;
        cg.alpha = shown ? 1 : 0;
        cg.blocksRaycasts = shown;
        cg.interactable = shown;
        if (shown)
        {
            clickAction.started += HandleClick;
        }
        else
        {
            clickAction.started -= HandleClick;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }

    private void HandleClick(InputAction.CallbackContext obj)
    {
        if (!isMouseOver)
        {
            HidePopup();
        }
    }
}
