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

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
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

        if (count == 10) SceneManager.LoadScene(2);
    }
}