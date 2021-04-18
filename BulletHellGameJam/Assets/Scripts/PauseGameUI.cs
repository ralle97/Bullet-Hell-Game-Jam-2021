using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGameUI : MonoBehaviour
{
    /*
    [SerializeField]
    private string mouseHoverSound = "ButtonHover";

    [SerializeField]
    private string buttonPressSound = "ButtonPress";
    */

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
        Debug.Log("To MainMenu!!!");
        // TODO: LoadScene("MainMenu");
    }

    public void Quit()
    {
        Debug.Log("APPLICATION QUIT!");
        Application.Quit();
    }

    public void Continue()
    {
        GameMaster.instance.TogglePauseMenu(false);
    }

    private void OnMouseOver()
    {
        // TODO: play mouseHover sound
    }
}
