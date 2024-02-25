using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DeathScreen : MonoBehaviour
{
    public float respawnDelay = 5f;
    public GameObject HUD_UI;
    public GameObject PauseMenu_UI;

    private void Start()
    {
        // if object is active when scene loads, deactivate object
        gameObject.SetActive(false);
        // hide and lock cursor
        CursorLockHandler.HideAndLockCursor();
    }

    private void OnEnable()
    {
        PauseMenu_UI.SetActive(false);
        HUD_UI.SetActive(false);
        CursorLockHandler.ShowAndUnlockCursor();
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

    public void ReloadScene()
    {
        StartCoroutine(DelayRespawn());
    }

    private IEnumerator DelayRespawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        SceneManager.LoadScene(2);
    }
}