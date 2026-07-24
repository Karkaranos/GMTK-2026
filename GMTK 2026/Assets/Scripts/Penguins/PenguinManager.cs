using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls giving all penguins their recommended parts.
/// </summary>
public class PenguinManager : Manager
{
    [SerializeField, Tooltip("Controls how many penguins recommend each part, with index 0 being the best choice, " +
        "index 1 being the second best, and so on.")] 
    private int[] favorRatio;
    [SerializeField] private PenguinRecDisplay recDisplay;

    private InputAction clickAction;
    private Penguin[] penguins;

    public override void Initialize()
    {
        clickAction = InputSystem.actions.FindAction("Click");
        clickAction.Enable();
        clickAction.started += HandleClick;
        penguins = GetComponentsInChildren<Penguin>(true);
        AssignParts();
    }

    private void OnDestroy()
    {
        clickAction.started -= HandleClick;
    }

    private void HandleClick(InputAction.CallbackContext obj)
    {
        Debug.Log(clickAction);
        if (PenguinRecDisplay.IsShown)
        {
            recDisplay.HandlePopupClick();
        }
        else if (Penguin.SelectedPenguin != null && !Penguin.SelectedPenguin.IsDistracted)
        {
            recDisplay.ShowPenguin(Penguin.SelectedPenguin);
        }
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

    //Returns the percent of penguins distracted.
    public float GetDistractedPercentage()
    {
        int distractedCount = 0;
        foreach (Penguin p in penguins)
        {
            if (p.IsDistracted)
                distractedCount++;
        }
        return distractedCount / penguins.Length;
    }
}
