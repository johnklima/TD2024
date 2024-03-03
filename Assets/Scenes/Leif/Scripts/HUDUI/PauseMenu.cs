using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{
    public GameObject HUD_UI;
    public GameObject PauseMenu_UI;
    public GameObject instructions;
    public GameObject credits;
    public bool isPaused;
    private CinemachineBrain _playerCam;

    private void Start()
    {
        _playerCam = FindObjectOfType<CinemachineBrain>();
        if (_playerCam == null)
            throw new Exception("Did not find <CinemachineBrain>, make sure there is one in the scene");
        PauseMenu_UI.SetActive(false);
    }

    public void OpenPauseMenu()
    {
        // unlock cursor
        // enable PauseMenu_UI
        PauseMenu_UI.SetActive(true);
        HUD_UI.SetActive(false);
        instructions.SetActive(false);
        credits.SetActive(false);
        _playerCam.enabled = false;
        CursorLockHandler.ShowAndUnlockCursor();
        Time.timeScale = 0;
    }

    public void ClosePauseMenu()
    {
        // lock cursor
        // disable PauseMenu_UI
        CursorLockHandler.HideAndLockCursor();
        instructions.SetActive(false);
        credits.SetActive(false);
        PauseMenu_UI.SetActive(false);
        _playerCam.enabled = true;
        HUD_UI.SetActive(true);
        Time.timeScale = 1;
    }

    public void OpenGameOverScreen()
    {
        OpenPauseMenu();
        credits.SetActive(true);
    }

    public void TogglePauseMenu()
    {
        // if input ESC
        //if credit or instr are open we are paused still

        if (credits.activeSelf || instructions.activeSelf)
            isPaused = true;
        else
            isPaused = !isPaused;

        // when pressing ESCAPE
        if (isPaused) CursorLockHandler.ShowAndUnlockCursor();
        else CursorLockHandler.HideAndLockCursor();
        Time.timeScale = isPaused ? 0 : 1;
        _playerCam.enabled = !isPaused;
        PauseMenu_UI.SetActive(isPaused);
        HUD_UI.SetActive(!isPaused); // toggle hud
        instructions.SetActive(false); // make sure instructions are off
        credits.SetActive(false); // make sure credits are off
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