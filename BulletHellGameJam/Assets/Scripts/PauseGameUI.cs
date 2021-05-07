using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private Button continueButton;

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
        continueButton.Select();
    }

    private void OnDisable()
    {
        GameMaster gm = GameMaster.instance;
        Cursor.SetCursor(gm.crosshairTexture, gm.crosshairHotspot, CursorMode.Auto);
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        audioManager.PlaySound(buttonPressSound);

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        sceneFader.FadeTo(menuSceneName);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        audioManager.PlaySound(buttonPressSound);

        //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

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
