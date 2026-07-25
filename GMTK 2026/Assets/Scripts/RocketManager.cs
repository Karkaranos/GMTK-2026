using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all information related to rocket scoring, including the current progress in the control room and
/// what parts are most optimal.
/// </summary>
public class RocketManager : Manager
{
    [SerializeField] private int[] partQualityMultipliers = new int[] { 3, 2, 1 };
    [SerializeField] private RocketPartDatabase parts;
    public static RocketManager Instance { get; private set;  }

    private readonly Dictionary<RocketSection, RocketPart[]> partScoringDict = new Dictionary<RocketSection, RocketPart[]>();

    public float Progress { get; set; }

    #region Properties
    public Dictionary<RocketSection, RocketPart[]> PartScoring => partScoringDict;
    #endregion

    public override void Initialize()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate RocketManager found.");
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        // Setup the scoring tiers for the rocket parts.
        foreach(var section in parts.Database)
        {
            RocketPart[] partsArray = new RocketPart[section.Parts.Length];
            section.Parts.CopyTo(partsArray, 0);
            ShuffleArray(partsArray);
            partScoringDict.Add(section.Section, partsArray);
            for(int i = 0; i < partsArray.Length; i++)
            {
                partsArray[i].Quality = partQualityMultipliers[i];
            }
        }

        DebugScoring();
    }

    public void DebugScoring()
    {
        string scoring = "";
        foreach(var section in partScoringDict.Keys)
        {
            scoring += $"{section}: ";
            for(int i = 0; i < partScoringDict[section].Length; i++)
            {
                scoring += $"\n    {i}. {partScoringDict[section][i].Name}.  ";
            }
            scoring += "\n\n";
        }
        Debug.Log(scoring);
    }

    /// <summary>
    /// Shuffles ana array using the fisher-yates algorithm.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    private static void ShuffleArray<T>(T[] array)
    {
        for(int i = array.Length - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            Swap(array, i, rand);
        }
    }

    private static void Swap<T>(T[] array, int index1, int index2)
    {
        T temp = array[index1];
        array[index1] = array[index2];
        array[index2] = temp;
    }
}
