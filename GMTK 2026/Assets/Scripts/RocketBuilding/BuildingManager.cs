using System.Collections.Generic;
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

    public Dictionary<RocketSection, RocketPart> GetParts()
    {
        Dictionary<RocketSection, RocketPart> parts = new();
        foreach(var section in sections)
        {
            if (!parts.ContainsKey(section.Section))
            {
                parts.Add(section.Section, section.Part);
            }
        }
        return parts;
    }

    public Dictionary<RocketSection, bool> CheckIssues()
    {
        Dictionary<RocketSection, bool> issues = new();
        foreach (var section in sections)
        {
            if (!issues.ContainsKey(section.Section))
            {
                issues.Add(section.Section, section.CheckIssue());
            }
        }
        return issues;
    }
}
