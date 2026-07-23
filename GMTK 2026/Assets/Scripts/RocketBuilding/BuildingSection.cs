using UnityEngine;

/// <summary>
/// Controls one section of the built rocket.
/// </summary>
public class BuildingSection : MonoBehaviour
{
    [SerializeField] private SpriteRenderer rend;
    [SerializeField] private RocketSection section;

    private RocketPart part;

    public RocketSection Section => section;

    public void SetPart(RocketPart part)
    {
        this.part = part;
        rend.sprite = part.Sprite;
    }
}
