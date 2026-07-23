using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls detecting clicks on penguins and showing the popup that gives ther recomendations.
/// </summary>
public class PenguinRecomendation : MonoBehaviour
{
    private Dictionary<RocketSection, RocketPart> recomendations;

    public Dictionary<RocketSection, RocketPart> Recommendations => recomendations;

    public void Initialize(Dictionary<RocketSection, RocketPart> recs)
    {
        recomendations = recs;
    }
}
