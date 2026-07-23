using UnityEngine;

[CreateAssetMenu(fileName = "RocketPartDatabase", menuName = "Scriptable Objects/RocketPartDatabase")]
public class RocketPartDatabase : ScriptableObject
{
    [SerializeField] private SectionDatabase[] partDatabase;

    public SectionDatabase[] Database => partDatabase;

    #region Nested
    [System.Serializable]
    public class SectionDatabase
    {
        [SerializeField] private RocketSection section;
        [SerializeField] private RocketPart[] parts;

        public RocketPart[] Parts => parts;
    }
    #endregion
}
