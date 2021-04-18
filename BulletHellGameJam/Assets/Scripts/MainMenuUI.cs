using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
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
        SceneManager.LoadScene(playLevelSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Application exiting...");
        Application.Quit();
    }

    private void OnMouseOver()
    {
        
    }
}
