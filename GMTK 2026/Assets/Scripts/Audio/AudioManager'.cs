/*****************************************
 * Author Name:     Cade Naylor           
 * Created Date:    7/23/2026
 * Modified Date:   7/23/2026
 * Description:     A base class to handle Audio
 ******************************************/
using FMOD.Studio;
using FMODUnity;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private Bus masterBus;
    private Bus sfxBus;
    private Bus musicBus;

    [Range(0, 1), HideInInspector]
    public float MasterVolume = 1f;
    [Range(0, 1), HideInInspector]
    public float SFXVolume = 1f;
    [Range(0, 1), HideInInspector]
    public float MusicVolume = 1f;

    private EventInstance backgroundMusic;


    /// <summary>
    /// Gets a reference to busses
    /// </summary>
    private void Start()
    {
        if (instance != null)
        {
            Debug.Log("There is more than one AudioManager in the scene");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        masterBus = RuntimeManager.GetBus("bus:/");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
        musicBus = RuntimeManager.GetBus("bus:/Music");

        UpdateVolume();
    }


    /// <summary>
    /// Play a one shot clip at the camera 
    /// </summary>
    /// <param name="sound">The sound to play</param>
    public void PlayOneShot(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);
    }

    /// <summary>
    /// Plays a sound at a provided position
    /// </summary>
    /// <param name="sound">Event reference</param>
    /// <param name="worldPos">Position to play the sound at</param>
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    /// <summary>
    /// Create an instance of the provided event
    /// </summary>
    /// <param name="eventReference">The event to create</param>
    /// <returns>a reference to the Event Instance</returns>
    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        //eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject.GetComponent<Transform>(), gameObject.GetComponent<Rigidbody>()));
        return eventInstance;
    }

    /// <summary>
    /// Updates the bus/mixer volumes to match the current in-game volumes
    /// </summary>
    public void UpdateVolume()
    {
        Debug.Log(SFXVolume);
        masterBus.setVolume(MasterVolume);
        sfxBus.setVolume(SFXVolume);
        musicBus.setVolume(MusicVolume);
    }

    /// <summary>
    /// Stops any background music when this object is destroyed
    /// </summary>
    private void OnDestroy()
    {
        backgroundMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}