using UnityEngine;

/// <summary>
/// Controls giving all penguins their recommended parts.
/// </summary>
public class PenguinManager : Manager
{
    private PenguinRecomendation[] penguins;

    public override void Initialize()
    {
        penguins = GetComponentsInChildren<PenguinRecomendation>(true);
        // Assign parts to all penguins.
    }
}
