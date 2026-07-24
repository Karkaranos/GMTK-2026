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
    [SerializeField, Foldout("Music")] public EventReference MainMenu;

    [SerializeField, Foldout("SFX")] public EventReference Bee;
    [SerializeField, Foldout("SFX")] public EventReference Meow;


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