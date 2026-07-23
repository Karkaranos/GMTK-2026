using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Controls giving all penguins their recommended parts.
/// </summary>
public class PenguinManager : Manager
{
    [SerializeField, Tooltip("Controls how many penguins recommend each part, with index 0 being the best choice, " +
        "index 1 being the second best, and so on.")] 
    private int[] favorRatio;

    private PenguinRecomendation[] penguins;

    public override void Initialize()
    {
        penguins = GetComponentsInChildren<PenguinRecomendation>(true);
        AssignParts();
    }

    private void AssignParts()
    {
        // Allocate the number of each part to be assigned.
        Dictionary<RocketSection, List<RocketPart>> partAssignments = GetPartAssignments();
        

        // Assign each penguin a random set of parts.
        foreach (var penguin in penguins)
        {
            Dictionary<RocketSection, RocketPart> recommendations = new Dictionary<RocketSection, RocketPart>();
            foreach (var type in partAssignments.Keys)
            {
                int randomPartIndex = Random.Range(0, partAssignments[type].Count);
                recommendations.Add(type, partAssignments[type][randomPartIndex]);
                partAssignments[type].RemoveAt(randomPartIndex);
            }
            penguin.Initialize(recommendations);
        }
    }

    /// <summary>
    /// Gets a dictionary storing a list of all part recommendations that need to be allocated.
    /// </summary>
    /// <returns></returns>
    private Dictionary<RocketSection, List<RocketPart>> GetPartAssignments()
    {
        Dictionary<RocketSection, List<RocketPart>> partAssignments = new Dictionary<RocketSection, List<RocketPart>>();
        foreach (var type in RocketManager.Instance.PartScoring.Keys)
        {
            RocketPart[] parts = RocketManager.Instance.PartScoring[type];
            List<RocketPart> partList = new List<RocketPart>();
            for (int i = 0; i < parts.Length; i++)
            {
                for (int j = 0; j < favorRatio[i]; j++)
                {
                    partList.Add(parts[i]);
                }
            }
            partAssignments.Add(type, partList);
        }
        return partAssignments;
    }
}
