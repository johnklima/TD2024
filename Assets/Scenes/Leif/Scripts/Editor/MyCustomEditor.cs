using Scenes.Leif.Scripts;
using UnityEditor;
using UnityEngine;

namespace Scenes.Leif.Editor
{
    // [CustomEditor(typeof(MyCustomClass))]
// public class MyCustomClassEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         var _target = target as MyCustomClass;
//          if (_target == null) return;
//          DrawDefaultInspector();
//         if (GUILayout.Button("Trigger 'Method()' on <_target>")) _target.Method();
//     }
// }

    [CustomEditor(typeof(InventoryController))]
    public class InventoryControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var _target = target as InventoryController;
            if (_target == null) return;
            DrawDefaultInspector();
            if (GUILayout.Button("Trigger TestOnInventoryChanged()")) _target.TestOnInventoryChanged();
        }
    }

    [CustomEditor(typeof(HealthDisplay))]
    public class HealthDisplayEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // TODO make test
            var _target = target as HealthDisplay;
            if (_target == null) return;
            DrawDefaultInspector();
            if (GUILayout.Button("trigger AddHeart()")) _target.TestAddHeart();
            if (GUILayout.Button("trigger RemoveHeart()")) _target.TestRemoveHeart();
        }
    }

    [CustomEditor(typeof(PlayerHealthSystem))]
    public class PlayerHealthSystemEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var _target = target as PlayerHealthSystem;
            if (_target == null) return;
            DrawDefaultInspector();
            if (GUILayout.Button("trigger TakeDamage()")) _target.TestTakeDamage();
            if (GUILayout.Button("trigger Heal()")) _target.TestHeal();
        }
    }


    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var _target = target as MapGenerator;
            if (_target == null) return;
            DrawDefaultInspector();
            if (GUILayout.Button("ReGenerate")) _target.ReGenerate();
        }
    }
}