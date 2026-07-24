/*****************************************
 * Author Name:     Cade Naylor           
 * Created Date:    7/23/2026
 * Modified Date:   7/24/2026
 * Description:     Stores functions called by menu buttons
 ******************************************/
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuBehavior : MonoBehaviour
{

    public static MenuBehavior Instance;
    public bool IsPaused = false;
    [SerializeField] private InputAction pauseAction;

    [SerializeField, Scene] private int[] gameScenes;
    [SerializeField, Scene] private int menuScene;

    [SerializeField, Required] private GameObject pauseMenu;
    [SerializeField, Required] private GameObject postLaunchMenu;
    [SerializeField, Required] private GameObject mainMenu;
    [SerializeField, Required] private GameObject controls;
    [SerializeField, Required] private GameObject credits;
    [SerializeField, Required] private GameObject settings;

    [SerializeField, Required] private Slider masterVolume;
    [SerializeField, Required] private Slider sfxVolume;
    [SerializeField, Required] private Slider musicVolume;

    public static bool GamePaused => Instance == null ? false : Instance.IsPaused;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        pauseAction.Enable();
        pauseAction.performed += HandlePauseInput;

        pauseMenu.SetActive(false);
        postLaunchMenu.SetActive(false);

        // If the current scene is the menu scene, enables the menu. Otherwise, disables it
        mainMenu.SetActive(SceneManager.GetActiveScene().buildIndex == menuScene);

        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

        masterVolume.value = AudioManager.instance.MasterVolume;
        sfxVolume.value = AudioManager.instance.SFXVolume;
        musicVolume.value = AudioManager.instance.MusicVolume;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
        pauseAction.performed -= HandlePauseInput;
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        mainMenu.SetActive(SceneManager.GetActiveScene().buildIndex == menuScene);
        if(IsPaused)
        {
            SetPaused(false);
        }
        postLaunchMenu.SetActive(false);
    }

    private void HandlePauseInput(InputAction.CallbackContext obj)
    {
        TogglePauseState();
    }

    /// <summary>
    /// Loads the game scene by the provided index
    /// </summary>
    public void LoadGameScene(int index)
    {
        credits.SetActive(false);
        controls.SetActive(false);
        SceneManager.LoadScene(gameScenes[index]);
    }

    /// <summary>
    /// Loads the menu scene by the provided index
    /// </summary>
    public void LoadMenuScene()
    {
        SceneManager.LoadScene(menuScene);
    }

    /// <summary>
    /// Quits the game if it isn't played in WebGL
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    /// <summary>
    /// Listens to pausing
    /// </summary>
    public void TogglePauseState()
    {
        SetPaused(!IsPaused);
    }

    public void SetPaused(bool paused)
    {
        if (SceneManager.GetActiveScene().buildIndex != menuScene && !postLaunchMenu.activeSelf)
        {
            IsPaused = paused;
            pauseMenu.SetActive(IsPaused);
            Time.timeScale = IsPaused ? 0 : 1;

            if (!IsPaused)
            {
                credits.SetActive(false);
                controls.SetActive(false);
                InputSystem.actions.Enable();
            }
            else
            {
                InputSystem.actions.Disable();
            }
        }
    }

    public void LaunchComplete()
    {
        postLaunchMenu.SetActive(true);
    }

    /// <summary>
    /// Toggles whether the credits are enabled or not
    /// </summary>
    public void ToggleCredits()
    {
        credits.SetActive(!credits.activeInHierarchy);
    }

    /// <summary>
    /// Toggles whether the controls are enabled or not
    /// </summary>
    public void ToggleControls()
    {
        controls.SetActive(!controls.activeInHierarchy);
    }

    /// <summary>
    /// Sets the master volume when the slider is updated
    /// </summary>
    public void SetMasterVolume(float volume)
    {
        AudioManager.instance.MasterVolume = volume;
        AudioManager.instance.UpdateVolume();
    }

    /// <summary>
    /// Sets the sound effect volume when the slider is updated
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        AudioManager.instance.SFXVolume = volume;
    }

    /// <summary>
    /// Sets the music volume when the slider is updated
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        AudioManager.instance.MusicVolume = volume;
        AudioManager.instance.UpdateVolume();
    }

    /// <summary>
    /// Toggles whether the credits are enabled or not
    /// </summary>
    public void ToggleSettings()
    {
        settings.SetActive(!settings.activeInHierarchy);
    }
}