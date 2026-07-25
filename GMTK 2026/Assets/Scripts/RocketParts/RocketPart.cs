
using NaughtyAttributes;
using UnityEngine;

[System.Serializable]
public class RocketPart
{
    [SerializeField] private string name;
    [SerializeField] private Sprite sprite;
    [SerializeField, ReadOnly, AllowNesting] 
        private float _quality = 2;

    #region GS
    public Sprite Sprite => sprite;
    public string Name => name;

    public float Quality { get => _quality; set => _quality = value;  }
    #endregion
}
