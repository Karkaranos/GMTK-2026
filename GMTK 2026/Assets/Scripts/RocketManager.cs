using UnityEngine;

/// <summary>
/// Manages all information related to rocket scoring, including the current progress in the control room and
/// what parts are most optimal.
/// </summary>
public class RocketManager : Manager
{
    [SerializeField] private RocketPartDatabase parts;
    private static RocketManager instance;

    public override void Initialize()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Duplicate RocketManager found.");
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }


    }
}
