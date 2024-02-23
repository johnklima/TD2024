#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ShowPopupExample : EditorWindow
{
    private void CreateGUI()
    {
        var label = new Label("This is an example of EditorWindow.ShowPopup");
        rootVisualElement.Add(label);

        var button = new Button();
        button.text = "Agree!";
        button.clicked += () => Close();
        rootVisualElement.Add(button);
    }

    [MenuItem("Examples/ShowPopup Example")]
    private static void Init()
    {
        var window = CreateInstance<ShowPopupExample>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        window.ShowPopup();
    }
}
#endif