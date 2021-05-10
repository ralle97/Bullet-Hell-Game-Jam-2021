using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private string mouseHoverSound = "ButtonHover";

    [SerializeField]
    private string buttonPressSound = "ButtonPress";

    public SceneFader sceneFader;

    public string playLevelSceneName = "MainLevel";

    [SerializeField]
    private string themeSong = "Music";

    private AudioManager audioManager;

    [SerializeField]
    private GameObject settingsMenu;

    [SerializeField]
    private TextMeshProUGUI howToPlay;

    private bool gamepadSupport = false;

    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Toggle settingsFullscreenToggle;
    [SerializeField]
    private TMP_Dropdown settingsQualityLevel;
    [SerializeField]
    private Slider settingsVolumeSlider;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            bool fullscreen = PlayerPrefs.GetInt("Fullscreen") == 1 ? true : false;

            Screen.fullScreen = fullscreen;

            if (fullscreen)
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            }
            else
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
            }

            settingsFullscreenToggle.isOn = fullscreen;
        }

        if (PlayerPrefs.HasKey("QualityLevel"))
        {
            int quality = PlayerPrefs.GetInt("QualityLevel");
            
            QualitySettings.SetQualityLevel(quality);

            settingsQualityLevel.value = quality;
        }

        if (PlayerPrefs.HasKey("ScreenWidth") && PlayerPrefs.HasKey("ScreenHeight"))
        {
            Screen.SetResolution(PlayerPrefs.GetInt("ScreenWidth"), PlayerPrefs.GetInt("ScreenHeight"), Screen.fullScreen);
        }

        if (PlayerPrefs.HasKey("Volume"))
        {
            float volume = PlayerPrefs.GetFloat("Volume");

            audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);

            settingsVolumeSlider.value = volume;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No AudioManager instance found in the scene");
        }

        audioManager.PlaySound(themeSong);

        ChangeHowToPlayText(gamepadSupport);
    }

    private void Update()
    {
        if (!gamepadSupport && Gamepad.all.Count > 0)
        {
            gamepadSupport = true;
            ChangeHowToPlayText(gamepadSupport);
        }

        if (gamepadSupport && Gamepad.all.Count == 0)
        {
            gamepadSupport = false;
            ChangeHowToPlayText(gamepadSupport);
        }
    }

    private void ChangeHowToPlayText(bool gamepadSupport)
    {
        if (!gamepadSupport)
        {
            howToPlay.text = "-<u>WASD or Arrows to move</u>\n" +
                             "-<u>Left mouse button to shoot</u>\n" + 
                             "-<u>Escape or P to pause the game</u>";
        }
        else
        {
            howToPlay.text = "-<u>Left stick to move</u>\n" +
                             "-<u>R2 to shoot, Right stick to aim</u>\n" +
                             "-<u>Start to pause the game</u>";
        }
    }

    public void PlayGame()
    {
        audioManager.PlaySound(buttonPressSound);

        audioManager.StopSound(themeSong);

        SceneManager.LoadScene(playLevelSceneName);
    }

    public void QuitGame()
    {
        audioManager.PlaySound(buttonPressSound);

        Debug.Log("Application exiting...");
        Application.Quit();
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(mouseHoverSound);
    }

    public void OpenSettingsMenu()
    {
        audioManager.PlaySound(buttonPressSound);

        settingsMenu.SetActive(true);
    }
}
