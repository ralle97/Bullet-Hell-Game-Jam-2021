using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    /*
    [SerializeField]
    private string mouseHoverSound = "ButtonHover";

    [SerializeField]
    private string buttonPressSound = "ButtonPress";
    */

    public string menuSceneName = "MainMenu";

    public SceneFader sceneFader;

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

    // TODO: AudioManager to play button press sound

    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("To MainMenu!!!");
        
        sceneFader.FadeTo(menuSceneName);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        Debug.Log("APPLICATION QUIT!");
        Application.Quit();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Debug.Log("Restart Cicked");
        
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    private void OnMouseOver()
    {
        // TODO: play mouseHover sound
    }
}
