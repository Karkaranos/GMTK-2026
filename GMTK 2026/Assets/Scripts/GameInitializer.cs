using UnityEngine;

/// <summary>
/// Controls initializing scripts in the correct order.
/// </summary>
public class GameInitializer : MonoBehaviour
{
    [SerializeField] private Manager[] managers;

    private void Awake()
    {
        foreach(var script in managers)
        {
            script.Initialize();
        }
    }
}
