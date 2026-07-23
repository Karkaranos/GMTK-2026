using UnityEngine;

[System.Serializable]
public class RocketPart
{
    [SerializeField] private string name;
    [SerializeField] private Sprite sprite;

    public Sprite Sprite => sprite;
    public string Name => name;
}
