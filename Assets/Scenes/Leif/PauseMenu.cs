using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{
    public GameObject HUD_UI;

    public GameObject PauseMenu_UI;

    private void Start()
    {
        PauseMenu_UI.SetActive(false);
    }

    public void OpenPauseMenu()
    {
        // unlock cursor
        // enable PauseMenu_UI
        PauseMenu_UI.SetActive(true);
        HUD_UI.SetActive(false);
        CursorLockHandler.ShowAndUnlockCursor();
        Time.timeScale = 0;
    }

    public void ClosePauseMenu()
    {
        // lock cursor
        // disable PauseMenu_UI
        CursorLockHandler.HideAndLockCursor();
        PauseMenu_UI.SetActive(false);
        HUD_UI.SetActive(true);
        Time.timeScale = 1;
    }

    public void TogglePauseMenu()
    {
        // lock cursor
        // disable PauseMenu_UI
        CursorLockHandler.ToggleState();
        HUD_UI.SetActive(!HUD_UI.activeSelf);
        PauseMenu_UI.SetActive(!PauseMenu_UI.activeSelf);
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }


    public void ExitGame()
    {
        StartCoroutine(TriggerExitGameDelayed());
    }

    private static IEnumerator TriggerExitGameDelayed()
    {
        Debug.Log("EXIT GAME");
        yield return new WaitForSecondsRealtime(1);
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}