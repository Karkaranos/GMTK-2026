using UnityEngine;

[CreateAssetMenu(fileName = "RocketPartDatabase", menuName = "Scriptable Objects/RocketPartDatabase")]
public class RocketPartDatabase : ScriptableObject
{
    [SerializeField] private RocketSectionDatabase[] partDatabase;

    public RocketSectionDatabase[] Database => partDatabase;

    #region Nested
    [System.Serializable]
    public class RocketSectionDatabase
    {
        [SerializeField] private RocketSection section;
        [SerializeField] private RocketPart[] parts;

        public RocketPart[] Parts => parts;
        public RocketSection Section => section;
    }
    #endregion
}
