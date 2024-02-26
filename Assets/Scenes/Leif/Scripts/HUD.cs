using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public UnityEvent OnUiButtonClicked;

    private void Start()
    {
        var buttons = FindObjectsOfType<Button>();

        foreach (var button in buttons) button.onClick.AddListener(ButtonClick);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void ButtonClick()
    {
        OnUiButtonClicked.Invoke();
    }
}