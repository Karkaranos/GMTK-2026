using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EndingRocketBuilder : MonoBehaviour
{
    [SerializeField, BoxGroup("Part Sprites")] private SpriteRenderer topRend;
    [SerializeField, BoxGroup("Part Sprites")] private SpriteRenderer wingRend;
    [SerializeField, BoxGroup("Part Sprites")] private SpriteRenderer engineRend;

    private Dictionary<RocketSection, RocketPart> parts;

    #region GS
    public Dictionary<RocketSection, RocketPart> Parts { get => parts; set => parts = value; }
    #endregion

    private void Awake()
    {
        parts = BuildingManager.SavedParts;

        RocketPart rp = parts[RocketSection.Top];
        topRend.sprite = rp == null ? null : rp.Sprite;
        rp = parts[RocketSection.Wings];
        wingRend.sprite = rp == null ? null : rp.Sprite;
        rp = parts[RocketSection.Engine];
        engineRend.sprite = rp == null ? null : rp.Sprite;
    }
}
