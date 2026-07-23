using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls detecting clicks on penguins and showing the popup that gives ther recomendations.
/// </summary>
public class Penguin : MonoBehaviour
{
    private Dictionary<RocketSection, RocketPart> recomendations;
    private PenguinRecDisplay display;

    public Dictionary<RocketSection, RocketPart> Recommendations => recomendations;

    public void Initialize(Dictionary<RocketSection, RocketPart> recs, PenguinRecDisplay display)
    {
        recomendations = recs;
        this.display = display;
    }

    /// <summary>
    /// Show the popup when the penguin is clicked on.
    /// </summary>
    private void OnMouseDown()
    {
        display.ShowPenguin(this);
    }
}
