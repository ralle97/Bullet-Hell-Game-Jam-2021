using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    [SerializeField]
    private GameObject settingsMenu;

    [SerializeField]
    private Button playButton;

    private AudioManager audioManager;

    [SerializeField]
    private string buttonPressSound = "ButtonPress";

    private Controls controls;

    private void Awake()
    {
        controls = new Controls();

        controls.Master.Back.performed += ctx => BackToMenu();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        audioManager = AudioManager.instance;

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int screenWidth = PlayerPrefs.GetInt("ScreenWidth", 0);
        int screenHeight = PlayerPrefs.GetInt("ScreenHeight", 0);

        int currentResolutionsIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (screenWidth == 0 || screenHeight == 0)
            {
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionsIndex = i;
                }
            }
            else
            {
                if (resolutions[i].width == screenWidth && resolutions[i].height == screenHeight)
                {
                    currentResolutionsIndex = i;
                }
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionsIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);

        PlayerPrefs.SetInt("QualityLevel", qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        if (isFullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }

        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        PlayerPrefs.SetInt("ScreenWidth", resolution.width);
        PlayerPrefs.SetInt("ScreenHeight", resolution.height);
    }

    public void BackToMenu()
    {
        PlayerPrefs.Save();

        audioManager.PlaySound(buttonPressSound);

        settingsMenu.SetActive(false);
        playButton.Select();
    }
}
