/*****************************************
 * Author Name:     Cade Naylor           
 * Created Date:    7/23/2026
 * Modified Date:   7/24/2026
 * Description:     Stores functions called by menu buttons
 ******************************************/
using System.Collections;
using NaughtyAttributes;
using TMPro;
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
    [SerializeField, Required] private Slider sensitivity;

    [SerializeField, BoxGroup("Launch")] private float distanceModifier = 150;
    [SerializeField, BoxGroup("Launch"), Required] private TMP_Text bigScore;
    [SerializeField, BoxGroup("Launch")] private float scoreTickSpeed = 150;
    [SerializeField, BoxGroup("Launch")] private float minScoreTickTime = 1;
    [SerializeField, BoxGroup("Launch")] private Vector2 scoreTickSize;
    [SerializeField, BoxGroup("Launch")] private AnimationCurve scoreTickCurve;
    [SerializeField, BoxGroup("Launch")] private float bigScorePauseTime;
    [SerializeField, BoxGroup("Launch")] private string postLaunchMessage;
    [SerializeField, BoxGroup("Launch"), Required] private TMP_Text postLaunchData;
    [SerializeField, BoxGroup("Launch")] private string newHighScoreMessage;
    [SerializeField, BoxGroup("Launch")] private string oldHighScoreMessage;
    [SerializeField, BoxGroup("Launch"), Required] private TMP_Text newHighScore;

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

        if (!PlayerPrefs.HasKey("sens")) PlayerPrefs.SetFloat("sens", 1);
        sensitivity.value = PlayerPrefs.GetFloat("sens");

        //StartCoroutine(LaunchComplete(1620));
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
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick);
        credits.SetActive(false);
        controls.SetActive(false);
        SceneManager.LoadScene(gameScenes[index]);
    }

    /// <summary>
    /// Loads the menu scene by the provided index
    /// </summary>
    public void LoadMenuScene()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick);
        SceneManager.LoadScene(menuScene);
    }

    /// <summary>
    /// Quits the game if it isn't played in WebGL
    /// </summary>
    public void QuitGame()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick);
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
            AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick);
            IsPaused = paused;
            pauseMenu.SetActive(IsPaused);
            Time.timeScale = IsPaused ? 0 : 1;

            if (!IsPaused)
            {
                credits.SetActive(false);
                controls.SetActive(false);
                settings.SetActive(false);
                InputSystem.actions.Enable();
            }
            else
            {
                InputSystem.actions.Disable();
            }
        }
    }

    public IEnumerator LaunchComplete(float flownHeight)
    {
        flownHeight *= distanceModifier;
        postLaunchMenu.SetActive(true);
        postLaunchData.gameObject.SetActive(false);
        newHighScore.gameObject.SetActive(false);
        bigScore.gameObject.SetActive(true);

        float currentHeight = 0;
        float maxAddedPerSecond = flownHeight / minScoreTickTime;
        while (currentHeight < flownHeight)
        {
            currentHeight += Time.deltaTime * Mathf.Min(scoreTickSpeed, maxAddedPerSecond) * scoreTickCurve.Evaluate(currentHeight / flownHeight);
            currentHeight = Mathf.Min(currentHeight, flownHeight);
            bigScore.text = (int)currentHeight + " km";
            bigScore.fontSize = scoreTickSize.x + ((scoreTickSize.y - scoreTickSize.x) * (currentHeight / flownHeight));
            yield return null;
        }

        yield return new WaitForSeconds(bigScorePauseTime);

        bigScore.gameObject.SetActive(false);
        postLaunchData.gameObject.SetActive(true);
        newHighScore.gameObject.SetActive(true);
        postLaunchData.text = postLaunchMessage.Replace("<height>", Mathf.RoundToInt(flownHeight).ToString());
        if (PlayerPrefs.HasKey("score") && PlayerPrefs.GetInt("score") >= Mathf.RoundToInt(flownHeight))
        {
            newHighScore.text = oldHighScoreMessage.Replace("<height>", PlayerPrefs.GetInt("score").ToString());
        }
        else
        {
            newHighScore.text = newHighScoreMessage;
            PlayerPrefs.SetInt("score", Mathf.RoundToInt(flownHeight));
        }
    }

    /// <summary>
    /// Toggles whether the credits are enabled or not
    /// </summary>
    public void ToggleCredits()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick);
        credits.SetActive(!credits.activeInHierarchy);
    }

    /// <summary>
    /// Toggles whether the controls are enabled or not
    /// </summary>
    public void ToggleControls()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick);
        controls.SetActive(!controls.activeInHierarchy);
    }

    /// <summary>
    /// Sets the master volume when the slider is updated
    /// </summary>
    public void SetMasterVolume(float volume)
    {
        AudioManager.instance.MasterVolume = volume;
        AudioManager.instance.UpdateVolume();
        AudioManager.instance.PlayOneShot(FMODEvents.instance.Master);
    }

    /// <summary>
    /// Sets the sound effect volume when the slider is updated
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        AudioManager.instance.SFXVolume = volume;
        AudioManager.instance.UpdateVolume();
        AudioManager.instance.PlayOneShot(FMODEvents.instance.SFX);
    }

    /// <summary>
    /// Sets the music volume when the slider is updated
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        AudioManager.instance.MusicVolume = volume;
        AudioManager.instance.UpdateVolume();
        AudioManager.instance.PlayOneShot(FMODEvents.instance.Music);
    }

    public void SetSensitivity()
    {
        PlayerPrefs.SetFloat("sens", sensitivity.value);
    }

    public void ClearHighScore()
    {
        PlayerPrefs.SetFloat("score", 0);
    }

    /// <summary>
    /// Toggles whether the credits are enabled or not
    /// </summary>
    public void ToggleSettings()
    {
        settings.SetActive(!settings.activeInHierarchy);
    }
}