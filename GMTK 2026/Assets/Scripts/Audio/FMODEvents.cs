/*****************************************
 * Author Name:     Cade Naylor           
 * Created Date:    7/23/2026
 * Modified Date:   7/23/2026
 * Description:     Stores all Event References
 ******************************************/
using UnityEngine;
using FMODUnity;
using NaughtyAttributes;

public class FMODEvents : MonoBehaviour
{
    [SerializeField, Foldout("Music")] public EventReference Beat;

    [SerializeField, Foldout("SFX")] public EventReference Bee;
    [SerializeField, Foldout("SFX")] public EventReference Meow;
    [SerializeField, Foldout("SFX")] public EventReference Pen;
    [SerializeField, Foldout("SFX")] public EventReference DavidNoise;
    [SerializeField, Foldout("SFX")] public EventReference ZachNoise;
    [SerializeField, Foldout("SFX")] public EventReference Tools;
    [SerializeField, Foldout("SFX")] public EventReference IceBreaks;
    [SerializeField, Foldout("SFX")] public EventReference UIClick;
    [SerializeField, Foldout("SFX")] public EventReference Fire;
    [SerializeField, Foldout("SFX")] public EventReference FireOut;

    [SerializeField, Foldout("Test Sounds")] public EventReference Master;
    [SerializeField, Foldout("Test Sounds")] public EventReference SFX;
    [SerializeField, Foldout("Test Sounds")] public EventReference Music;


    public static FMODEvents instance { get; private set; }

    /// <summary>
    /// Creates a single instance
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

}