using UnityEngine;

[System.Serializable]
public class RocketPart
{
    [SerializeField] private string name;
    [SerializeField] private Sprite sprite;
    [SerializeField, Range(1, 3)] 
        private float _quality = 2;

    #region GS
    public Sprite Sprite => sprite;
    public string Name => name;

    public float Quality => _quality;
    #endregion
}
