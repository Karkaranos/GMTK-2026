using UnityEngine;

/// <summary>
/// Controls one section of the built rocket.
/// </summary>
public class BuildingSection : MonoBehaviour
{
    [SerializeField] private SpriteRenderer rend;
    [SerializeField] private RocketSection section;
    [field: SerializeField] public ProgressBar BuildingBar { get; set; }

    private RocketPart part;
    private ShipEventBubbleSpawner[] eventSpawners;

    public RocketSection Section => section;
    public RocketPart Part => part;

    private void Awake()
    {
        eventSpawners = GetComponentsInChildren<ShipEventBubbleSpawner>();
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
        ToggleEventSpawners(true);
    }

    public void OnEndBuild()
    {
        ToggleEventSpawners(false);
    }
}
