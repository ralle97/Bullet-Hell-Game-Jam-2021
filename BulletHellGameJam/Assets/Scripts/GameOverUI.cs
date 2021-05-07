using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private string mouseHoverSound = "ButtonHover";

    [SerializeField]
    private string buttonPressSound = "ButtonPress";

    public string menuSceneName = "MainMenu";

    public SceneFader sceneFader;

    private AudioManager audioManager;

    [SerializeField]
    private Button restartButton;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No AudioManager instance found in the scene");
        }
    }

    private void OnEnable()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        restartButton.Select();
    }

    private void OnDisable()
    {
        GameMaster gm = GameMaster.instance;
        Cursor.SetCursor(gm.crosshairTexture, gm.crosshairHotspot, CursorMode.Auto);
    }

    public void ToMainMenu()
    {
        audioManager.PlaySound(buttonPressSound);

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        sceneFader.FadeTo(menuSceneName);
    }

    public void Quit()
    {
        audioManager.PlaySound(buttonPressSound);

        //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        Debug.Log("APPLICATION QUIT!");
        Application.Quit();
    }

    public void Restart()
    {
        audioManager.PlaySound(buttonPressSound);
        
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(mouseHoverSound);
    }
}
