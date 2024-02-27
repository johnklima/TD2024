using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FakeLoading : MonoBehaviour
{
    public string loadingText = "Loading";
    public int numberOfDots = 3;
    public float loadingSpeed = 1;
    private TMP_Text _text;
    private int count;
    private string dots = ".";

    private float lerp;
    private MainMenu mainMenu;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();


        // we have been loaded additively, disable this cam
        Debug.Log("Loaded additively, loading screen");
        // StartCoroutine(WaitForFakeLoading());
    }

    private void Update()
    {
        if (mainMenu == null)
        {
            mainMenu = FindObjectOfType<MainMenu>();
            mainMenu.doneLoading.AddListener(OnDoneLoading);
            Debug.LogError("Main Menu found!!!!");
            return;
        }

        lerp += Time.deltaTime * loadingSpeed;
        if (lerp >= 1)
        {
            lerp = 0;
            dots += ".";
            count++;
        }

        if (dots.Length > numberOfDots)
            dots = ".";


        _text.text = loadingText + dots;
    }

    private void OnDoneLoading()
    {
        Debug.Log("OnDoneLoading loadingScene next scene");
        SceneManager.LoadScene(2);
    }

    private IEnumerator WaitForFakeLoading()
    {
        yield return new WaitForSeconds(mainMenu.loadingScreenDuration);
        SceneManager.LoadScene(2);
    }
}