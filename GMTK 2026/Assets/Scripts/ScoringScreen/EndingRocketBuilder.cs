using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EndingRocketBuilder : MonoBehaviour
{
    [SerializeField, BoxGroup("Part Sprites")] private SpriteRenderer topRend;
    [SerializeField, BoxGroup("Part Sprites")] private SpriteRenderer wingRend;
    [SerializeField, BoxGroup("Part Sprites")] private SpriteRenderer engineRend;


    private void Awake()
    {
        Dictionary<RocketSection, RocketPart> parts = BuildingManager.SavedParts;

        RocketPart rp = parts[RocketSection.Top];
        topRend.sprite = rp == null ? null : rp.Sprite;
        rp = parts[RocketSection.Wings];
        wingRend.sprite = rp == null ? null : rp.Sprite;
        rp = parts[RocketSection.Engine];
        engineRend.sprite = rp == null ? null : rp.Sprite;
    }
}
