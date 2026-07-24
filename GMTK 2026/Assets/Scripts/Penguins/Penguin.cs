using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls detecting clicks on penguins and showing the popup that gives ther recomendations.
/// </summary>
public class Penguin : MonoBehaviour
{
    [SerializeField] private Color selectedColor;

    [SerializeField, BoxGroup("Components")] private SpriteRenderer rend;

    private Dictionary<RocketSection, RocketPart> recomendations;

    private bool isDistracted  = false;
    private Color baseColor;

    private static Penguin selectedPenguin;
    private static Penguin mouseOverPenguin;

    public static Penguin SelectedPenguin
    {
        get { return selectedPenguin; }
        private set
        {
            if (selectedPenguin != null)
            {
                selectedPenguin.OnDeselected();
            }

            selectedPenguin = value;

            if (selectedPenguin != null)
            {
                selectedPenguin.OnSelected();
            }
        }
    }

    #region GS
    public Dictionary<RocketSection, RocketPart> Recommendations => recomendations;

    public bool IsDistracted { get => isDistracted; set => isDistracted = value; }
    #endregion

    private void Reset()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Dictionary<RocketSection, RocketPart> recs)
    {
        baseColor = rend.color;
        recomendations = recs;
    }

    public void OnSelected()
    {
        rend.color = selectedColor;
    }

    public void OnDeselected()
    {
        rend.color = baseColor;
    }

    private void OnMouseEnter()
    {
        mouseOverPenguin = this;
        if (!PenguinRecDisplay.IsShown && !isDistracted && !MenuBehavior.GamePaused)
        {
            SelectedPenguin = this;
        }
    }

    private void OnMouseExit()
    {
        if (mouseOverPenguin == this)
        {
            mouseOverPenguin = null;
        }
        if (SelectedPenguin == this && !PenguinRecDisplay.IsShown)
        {
            SelectedPenguin = null;
        }
    }

    public static void ResetSelectedPenguin()
    {
        SelectedPenguin = mouseOverPenguin;
    }
}
