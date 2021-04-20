using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private string mouseHoverSound = "ButtonHover";

    [SerializeField]
    private string buttonPressSound = "ButtonPress";

    public SceneFader sceneFader;

    public string playLevelSceneName = "MainLevel";

    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No AudioManager instance found in the scene");
        }
    }

    public void PlayGame()
    {
        audioManager.PlaySound(buttonPressSound);

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
        Debug.Log("Sound should play");

        audioManager.PlaySound(mouseHoverSound);
    }
}
