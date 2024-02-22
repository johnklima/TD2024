#if UNITY_EDITOR
using System;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class CheatPanel : EditorWindow
{
    public Vector3 spwnPos;

    private readonly Color defaultHrColor = new(103 / 255f, 103 / 255f, 103 / 255f);

    private readonly string[] options =
    {
        "Blue_Ingredient",
        "Red_Ingredient",
        "Skull_Ingredient",
        "Sun_Ingredient",
        "Whirlpool_Ingredient",
        "Yellow_Ingredient",
        "Blue_Blue_Potion",
        "Blue_Skull_Potion",
        "Blue_Sun_Potion",
        "Blue_Whirlpool_Potion",
        "Red_Red_Potion",
        "Red_Skull_Potion",
        "Red_Sun_Potion",
        "Red_Whirlpool_Potion",
        "Yellow_Skull_Potion",
        "Yellow_Sun_Potion",
        "Yellow_Whirlpool_Potion",
        "Yellow_Yellow_Potion"
    };

    private int index;

    private void OnGUI()
    {
        var lPad = position.width / 16;
        var rPad = lPad * 2;
        var currHeight = 10;
        if (!Application.isPlaying) DrawHorizontalLine(Color.red, "Can only test during RunTime");

        #region Item Tests

        DrawHorizontalLine(defaultHrColor, "Item Spawning");
        //dropdown menu
        index = EditorGUI.Popup(new Rect(
                lPad, currHeight,
                position.width - rPad, 25),
            "Item:", index, options);
        currHeight += 30;
        // spawn in inv button
        if (GUI.Button(new Rect(lPad, currHeight, position.width - rPad, 25),
                "Spawn in Inventory")) SpawnInInventory();
        currHeight += 30;
        // spawn in world0 button
        if (GUI.Button(new Rect(lPad, currHeight, position.width - rPad, 25),
                "Spawn in world (0,0,0)")) SpawnInWorld(Vector3.zero);
        currHeight += 30;
        spwnPos = EditorGUI.Vector3Field(new Rect(
                position.width / 2 - rPad / 2, currHeight + 5,
                position.width / 2, 25),
            string.Empty, spwnPos);
        // spawn on spawnPos button
        if (GUI.Button(new Rect(lPad, currHeight,
                    position.width / 2 - rPad, 25),
                "Spawn at Pos")) SpawnInWorld(spwnPos);
        currHeight += 30;
        // spawn on crosshair button
        if (GUI.Button(new Rect(lPad, currHeight,
                    position.width - rPad, 25),
                "Spawn in world (Crosshair)"))
        {
            var camT = Camera.main.transform;
            if (camT != null)
                if (Physics.Raycast(camT.position, camT.forward, out var hit))
                    SpawnInWorld(hit.point);
        }

        currHeight += 30;

        #endregion

        #region HealthDisplay Tests

        DrawHorizontalLine(defaultHrColor);
        DrawHorizontalLine(defaultHrColor, "HealthDisplay Tests");
        if (GUI.Button(new Rect(lPad, currHeight, position.width / 2 - rPad / 2, 25),
                "Trigger AddHeart()")) TriggerAddHeart();
        if (GUI.Button(new Rect(position.width / 2, currHeight, position.width / 2 - rPad / 2, 25),
                "Trigger RemoveHeart()")) TriggerRemoveHeart();
        currHeight += 30;

        #endregion

        #region PlayerHealthSystem

        DrawHorizontalLine(defaultHrColor);
        DrawHorizontalLine(defaultHrColor, "PlayerHealthSystem Tests");

        if (GUI.Button(new Rect(lPad, currHeight, position.width / 2 - rPad / 2, 25),
                "Trigger Heal()")) TriggerHeal();
        if (GUI.Button(new Rect(position.width / 2, currHeight, position.width / 2 - rPad / 2, 25),
                "Trigger TakeDamage()")) TriggerTakeDamage();
        currHeight += 30;

        #endregion

        #region DrawHorizontalLine

        void DrawHorizontalLine(Color color, string label = "")
        {
            var h = 1;
            if (label.Length > 0) h = 14;
            currHeight += 3;
            EditorGUI.DrawRect(new Rect(lPad / 2 - 4, currHeight - 4,
                    position.width - rPad / 2 + 6, label.Length > 0 ? h + 6 : h),
                color + new Color(.2f, .2f, .2f));
            EditorGUI.DrawRect(new Rect(lPad / 2 - 3, currHeight - 3,
                    position.width - rPad / 2 + 5, label.Length > 0 ? h + 5 : h),
                color + new Color(.05f, .05f, .05f));
            EditorGUI.DrawRect(new Rect(lPad / 2 - 2, currHeight - 2,
                    position.width - rPad / 2 + 2, label.Length > 0 ? h + 2 : h),
                color);
            if (label.Length > 0)
            {
                EditorGUI.LabelField(new Rect(lPad / 2 + 2, currHeight,
                    position.width - rPad / 2 - 2, 15), label);
                currHeight += 15;
            }

            currHeight += 5;
        }

        DrawHorizontalLine(defaultHrColor);

        #endregion
    }


    public void TriggerTakeDamage()
    {
        if (TriedWhileNotPlayMode()) return;
        var pHs = FindObjectOfType<PlayerHealthSystem>();
        if (pHs == null) throw new Exception("Make sure there is a <PlayerHealthSystem> component in the scene");
        Debug.Log("Performed Test on: <PlayerHealthSystem.TakeDamage()> with a value of 1");
        pHs.TakeDamage(1);
    }

    public void TriggerHeal()
    {
        if (TriedWhileNotPlayMode()) return;
        var pHs = FindObjectOfType<PlayerHealthSystem>();
        if (pHs == null) throw new Exception("Make sure there is a <PlayerHealthSystem> component in the scene");
        Debug.Log("Performed Test on: <PlayerHealthSystem.Heal()> with a value of 1");
        pHs.Heal(1);
    }

    public void TriggerAddHeart()
    {
        if (TriedWhileNotPlayMode()) return;
        var h = FindObjectOfType<HealthDisplay>();
        var pHs = FindObjectOfType<PlayerHealthSystem>();

        if (h == null || pHs == null)
            throw new Exception("Make sure there is a <HealthDisplay> & <PlayerHealthSystem> component in the scene");
        Debug.Log("Performed Test on: <HealthDisplay.AddHeart()>");
        h.TestAddHeart();
    }

    public void TriggerRemoveHeart()
    {
        if (TriedWhileNotPlayMode()) return;
        var h = FindObjectOfType<HealthDisplay>();
        if (h == null) throw new Exception("Make sure there is a <HealthDisplay> component in the scene");
        Debug.Log("Performed Test on: <HealthDisplay.RemoveHeart()>");
        h.TestRemoveHeart();
    }

    private bool TriedWhileNotPlayMode()
    {
        if (Application.isPlaying) return false;
        Debug.LogError("Can only spawn during RunTime");
        return true;
    }

    private void SpawnInInventory()
    {
        if (TriedWhileNotPlayMode()) return;
        // add item
        var invController = FindObjectOfType<InventoryController>();
        if (invController == null)
        {
            Debug.LogError("No InventoryController to spawn items in");
            return;
        }

        var prefabs = Resources.LoadAll<GameObject>("Interactable Items/Ingredients")
            .Concat(Resources.LoadAll<GameObject>("Interactable Items/Potions")).ToArray();
        invController.AddItem(prefabs[index].GetComponent<Item>());
        Debug.Log($"Added {prefabs[index].name} to inventory");
    }

    private void SpawnInWorld(Vector3 pos)
    {
        if (TriedWhileNotPlayMode()) return;
        var prefabs = Resources.LoadAll<GameObject>("Interactable Items/Ingredients")
            .Concat(Resources.LoadAll<GameObject>("Interactable Items/Potions")).ToArray();
        Instantiate(prefabs[index], pos, quaternion.identity);
        Debug.Log($"Spawned {prefabs[index].name} @ {pos}");
    }


    [MenuItem("LEIF TOOLS/CHEATS")]
    private static void Init()
    {
        var window = GetWindow<CheatPanel>();
        var w = 350;
        var h = 300;

        window.position = new Rect(
            200, 50,
            w, h
        );
        window.Show();
    }
}
#endif