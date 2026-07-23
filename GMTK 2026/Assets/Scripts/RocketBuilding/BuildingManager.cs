using UnityEngine;

/// <summary>
/// Manages collecting all info about the built rocket and sending it to the RocketManager
/// </summary>
public class BuildingManager : Manager
{
    private BuildingSection[] sections;

    public override void Initialize()
    {
        sections = GetComponentsInChildren<BuildingSection>(true);
    }
}
