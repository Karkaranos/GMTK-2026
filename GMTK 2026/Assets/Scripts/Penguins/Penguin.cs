using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls detecting clicks on penguins and showing the popup that gives ther recomendations.
/// </summary>
public class Penguin : MonoBehaviour
{
    [SerializeField] private Color selectedColor;

    [SerializeField, BoxGroup("Components")] private SpriteRenderer rend;
    [SerializeField, BoxGroup("Components")] private Animator anim;

    private Dictionary<RocketSection, RocketPart> recomendations;

    private bool isDistracted  = false;
    private Color baseColor;
    private Material outlineMat;

    private static Penguin selectedPenguin;
    private static Penguin mouseOverPenguin;

    private Image perSecondImage;

    private int distractionCount = 0;

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

    public bool IsDistracted
    { get => isDistracted;
        set
        { 
            isDistracted = value; 
            perSecondImage.enabled = !isDistracted;
        }
    }

    public Image PerSecondImage { get => perSecondImage; set => perSecondImage = value; }

    #endregion

    private void Reset()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Dictionary<RocketSection, RocketPart> recs, Image psImage)
    {
        outlineMat = rend.material;
        baseColor = outlineMat.GetColor("_OutlineColor");
        recomendations = recs;

        perSecondImage = psImage;
        rend.material = outlineMat;
    }

    public void OnSelected()
    {
        outlineMat.SetColor("_OutlineColor", selectedColor);
        //rend.color = selectedColor;
    }

    public void OnDeselected()
    {
        outlineMat.SetColor("_OutlineColor", baseColor);
        //rend.color = baseColor;
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

    public void AddDistraction(string distractionTrigger)
    {
        anim.SetBool("IsDistracted", true);
        anim.SetTrigger(distractionTrigger);
        distractionCount++;
        IsDistracted = distractionCount > 0;
    }

    public void RemoveDistraction()
    {
        anim.SetBool("IsDistracted", false);
        distractionCount = Mathf.Max(0, distractionCount - 1);
        IsDistracted = distractionCount > 0;
    }
}
