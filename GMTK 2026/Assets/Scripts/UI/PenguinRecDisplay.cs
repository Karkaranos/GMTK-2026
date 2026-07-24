using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PenguinRecDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private RecReferences[] recReferences;

    private bool isMouseOver;
    private static bool isShown;

    public static bool IsShown => isShown;

    #region Nested
    [System.Serializable]
    private struct RecReferences
    {
        [SerializeField] internal RocketSection section;
        [SerializeField] internal Image image;
    }
    #endregion

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
        Penguin.ResetSelectedPenguin();
        TogglePopup(false);
    }

    public void TogglePopup(bool shown)
    {
        SceneSwitcher.CanSwitchScenes = !shown;
        isShown = shown;
        cg.alpha = shown ? 1 : 0;
        cg.blocksRaycasts = shown;
        cg.interactable = shown;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }

    public void HandlePopupClick()
    {
        if (!isMouseOver)
        {
            HidePopup();
        }
    }
}
