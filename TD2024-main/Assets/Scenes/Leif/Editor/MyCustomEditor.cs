using Scenes.Leif.Scripts;
using UnityEditor;
using UnityEngine;

// [CustomEditor(typeof(MyCustomClass))]
// public class MyCustomEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         var target = (MyCustomClass)target;
//         DrawDefaultInspector();
//         if (GUILayout.Button("Trigger "Method()" on <target>")) target.Method();
//     }
// }

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var _target = (MapGenerator)target;
        DrawDefaultInspector();
        if (GUILayout.Button("ReGenerate")) _target.ReGenerate();
    }
}