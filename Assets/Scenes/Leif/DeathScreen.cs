using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DeathScreen : MonoBehaviour
{
    public float respawnDelay = 5f;

    private void Start()
    {
        gameObject.SetActive(false);
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