using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject InstructionPanel;
    public GameObject Creditspanel;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OnInstructions()
    {
        Debug.Log("heihei");
    }

    public void OnCredits()
    {

        Debug.Log("hei");
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
