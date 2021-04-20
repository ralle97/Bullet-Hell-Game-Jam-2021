using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGameUI : MonoBehaviour
{
    [SerializeField]
    private string mouseHoverSound = "ButtonHover";

    [SerializeField]
    private string buttonPressSound = "ButtonPress";

    public SceneFader sceneFader;

    [SerializeField]
    private string menuSceneName = "MainMenu";

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

    public void ToMainMenu()
    {
        audioManager.PlaySound(buttonPressSound);

        sceneFader.FadeTo(menuSceneName);
    }

    public void Quit()
    {
        audioManager.PlaySound(buttonPressSound);

        Debug.Log("APPLICATION QUIT!");
        Application.Quit();
    }

    public void Continue()
    {
        audioManager.PlaySound(buttonPressSound);

        GameMaster.instance.TogglePauseMenu(false);
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(mouseHoverSound);
    }
}
