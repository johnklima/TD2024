using UnityEditor;
using UnityEngine;

namespace Scenes.Leif.Scripts.Editor
{
    // [CustomEditor(typeof(MyCustomClass))]
// public class MyCustomClassEditor : UnityEditor.Editor
// {
//     public override void OnInspectorGUI()
//     {
//         var _target = target as MyCustomClass;
//          if (_target == null) return;
//          DrawDefaultInspector();
//         if (GUILayout.Button("Trigger 'Method()' on <_target>")) _target.Method();
//     }
// }
//
//
    // [CustomEditor(typeof(PotionObjectItem))]
    // public class PotionObjectItemEditor : UnityEditor.Editor
    // {
    //     public override void OnInspectorGUI()
    //     {
    //         var _target = target as PotionObjectItem;
    //         if (_target == null) return;
    //         DrawDefaultInspector();
    //         if (GUILayout.Button("Trigger 'Method()' on <_target>")) _target.CopyData();
    //     }
    // }
    //
    // [CustomEditor(typeof(IngredientObjectItem))]
    // public class IngredientObjectItemEditor : UnityEditor.Editor
    // {
    //     public override void OnInspectorGUI()
    //     {
    //         var _target = target as IngredientObjectItem;
    //         if (_target == null) return;
    //         DrawDefaultInspector();
    //         if (GUILayout.Button("Trigger 'Method()' on <_target>")) _target.CopyData();
    //     }
    // }
    [CustomEditor(typeof(GameOverArea))]
    public class GameOverAreaEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var _target = target as GameOverArea;
            if (_target == null) return;
            DrawDefaultInspector();
            if (GUILayout.Button("Game Over!")) _target.TestOnGameOverEnter();
        }
    }

    [CustomEditor(typeof(AmbianceMusicHandler))]
    public class AmbianceMusicHandlerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var _target = target as AmbianceMusicHandler;
            if (_target == null) return;
            DrawDefaultInspector();
            if (GUILayout.Button("NextClip()")) _target.NextClip();
        }
    }

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