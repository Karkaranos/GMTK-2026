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

        topRend.sprite = parts[RocketSection.Top].Sprite;
        wingRend.sprite = parts[RocketSection.Wings].Sprite;
        engineRend.sprite = parts[RocketSection.Engine].Sprite;
    }
}
