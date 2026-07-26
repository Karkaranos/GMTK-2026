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
    [SerializeField, Foldout("Music")] public EventReference BadEndingMusic;
    [SerializeField, Foldout("Music")] public EventReference GameplayMusic;
    [SerializeField, Foldout("Music")] public EventReference MenuMusic;

    [SerializeField, Foldout("SFX")] public EventReference Bee;
    [SerializeField, Foldout("SFX")] public EventReference Meow;
    [SerializeField, Foldout("SFX")] public EventReference CoffeeSpill;
    [SerializeField, Foldout("SFX")] public EventReference DavidNoise;
    [SerializeField, Foldout("SFX")] public EventReference Fire;
    [SerializeField, Foldout("SFX")] public EventReference FireOut;
    [SerializeField, Foldout("SFX")] public EventReference IceFall;
    [SerializeField, Foldout("SFX")] public EventReference IceBreak;
    [SerializeField, Foldout("SFX")] public EventReference IceBreakOff;
    [SerializeField, Foldout("SFX")] public EventReference PenClick;
    [SerializeField, Foldout("SFX")] public EventReference Tools;
    [SerializeField, Foldout("SFX")] public EventReference UIClick;
    [SerializeField, Foldout("SFX")] public EventReference ZachNoise;
    [SerializeField, Foldout("SFX")] public EventReference Game;
    [SerializeField, Foldout("SFX")] public EventReference ComputerGlitch;
    [SerializeField, Foldout("SFX")] public EventReference Beat;
    [SerializeField, Foldout("SFX")] public EventReference Launch;
    [SerializeField, Foldout("SFX")] public EventReference Cheer;
    [SerializeField, Foldout("SFX")] public EventReference Explosion;
    [SerializeField, Foldout("SFX")] public EventReference Snoring;
    [SerializeField, Foldout("SFX")] public EventReference WakeUp;

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