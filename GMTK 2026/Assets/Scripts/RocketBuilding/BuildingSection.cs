using NaughtyAttributes;
using System;
using UnityEngine;

/// <summary>
/// Controls one section of the built rocket.
/// </summary>
public class BuildingSection : MonoBehaviour
{
    [SerializeField, BoxGroup("Components")] private SpriteRenderer rend;
    [SerializeField] private RocketSection section;
    [SerializeField, BoxGroup("References")] private ParticleSystem buildingParticles;
    [field: SerializeField, BoxGroup("References")] public ProgressBar BuildingBar { get; set; }

    private RocketPart part;
    private ShipEventBubbleSpawner[] eventSpawners;

    public RocketSection Section => section;
    public RocketPart Part => part;

    private void Awake()
    {
        eventSpawners = GetComponentsInChildren<ShipEventBubbleSpawner>(true);
        ToggleEventSpawners(false);
    }

    public void SetPart(RocketPart part)
    {
        this.part = part;
        rend.sprite = part.Sprite;
    }

    /// <summary>
    /// Increases/decreases the build rate of this part.
    /// </summary>
    /// <param name="buildRate"></param>
    public void AddBuildRate(int buildRate)
    {
        BuildingBar.FillRate += buildRate;
    }

    private void ToggleEventSpawners(bool isSpawning)
    {
        foreach (var eventSpawner in eventSpawners)
        {
            eventSpawner.enabled = isSpawning;
        }
    }

    public void OnBeginBuild()
    {
        buildingParticles.Play();
        ToggleEventSpawners(true);
    }

    public bool CheckIssue()
    {
        bool hasIssue = false;
        foreach(var eventSpawner in eventSpawners)
        {
            hasIssue |= eventSpawner.ActiveBubbleNum > 0;
        }
        return hasIssue;
    }

    public void OnEndBuild()
    {
        buildingParticles.Stop();
        ToggleEventSpawners(false);
        // Clear all bubbles.
        foreach (var eventSpawner in eventSpawners)
        {
            eventSpawner.PopAllBubbles();
        }
    }
}
