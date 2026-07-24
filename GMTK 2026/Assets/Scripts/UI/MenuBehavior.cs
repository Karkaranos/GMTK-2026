/*****************************************
 * Author Name:     Cade Naylor           
 * Created Date:    7/23/2026
 * Modified Date:   7/23/2026
 * Description:     Stores functions called by menu buttons
 ******************************************/
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehavior : MonoBehaviour
{

    public static MenuBehavior Instance;
    public bool IsPaused = false;

    [SerializeField, Scene] private int gameScene;
    [SerializeField, Scene] private int menuScene;

    [SerializeField, Required] private GameObject pauseMenu;
    [SerializeField, Required] private GameObject mainMenu;
    [SerializeField, Required] private GameObject controls;
    [SerializeField, Required] private GameObject credits;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        pauseMenu.SetActive(false);

        // If the current scene is the menu scene, enables the menu. Otherwise, disables it
        mainMenu.SetActive(SceneManager.GetActiveScene().buildIndex == menuScene);

        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        mainMenu.SetActive(SceneManager.GetActiveScene().buildIndex == menuScene);
        if(IsPaused)
        {
            pauseMenu.SetActive(false);
        }
    }

    /// <summary>
    /// Loads the game scene by the provided index
    /// </summary>
    public void LoadGameScene()
    {
        credits.SetActive(false);
        controls.SetActive(false);
        SceneManager.LoadScene(gameScene);
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
        if(SceneManager.GetActiveScene().buildIndex == gameScene)
        {
            IsPaused = !IsPaused;
            pauseMenu.SetActive(IsPaused);

            if(!IsPaused)
            {
                credits.SetActive(false);
                controls.SetActive(false);
            }
        }
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


}