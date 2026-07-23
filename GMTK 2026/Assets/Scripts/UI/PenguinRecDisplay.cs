using UnityEngine;
using UnityEngine.UI;

public class PenguinRecDisplay : MonoBehaviour
{
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private RecReferences[] recReferences;

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
        TogglePopup(false);
    }

    public void TogglePopup(bool shown)
    {
        SceneSwitcher.CanSwitchScenes = !shown;
        cg.alpha = shown ? 1 : 0;
        cg.blocksRaycasts = shown;
        cg.interactable = shown;

    }
}
