#if UNITY_EDITOR
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class CheatPanel : EditorWindow
{
    public Vector3 spwnPos;

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
    private bool triedWhileNotPlayMode;

    private void OnGUI()
    {
        var lPad = position.width / 16;
        var rPad = lPad * 2;
        var currHeight = 10;
        if (triedWhileNotPlayMode)
        {
            EditorGUI.TextArea(new Rect(lPad, currHeight, position.width - rPad, 25),
                "Can only spawn during RunTime");
            currHeight += 40;
        }

        index = EditorGUI.Popup(
            new Rect(
                lPad, currHeight,
                position.width - rPad,
                25
            ),
            "Item:",
            index,
            options);
        currHeight += 30;

        if (GUI.Button(new Rect(lPad, currHeight, position.width - rPad, 25),
                "Spawn in Inventory")) SpawnInInventory();
        currHeight += 30;

        if (GUI.Button(new Rect(lPad, currHeight, position.width - rPad, 25),
                "Spawn in world (0,0,0)")) SpawnInWorld(Vector3.zero);
        currHeight += 30;


        spwnPos = EditorGUI.Vector3Field(new Rect(
                position.width / 2 - rPad / 2, currHeight + 5,
                position.width / 2,
                25
            ),
            "", spwnPos);
        // currHeight += 55;

        if (GUI.Button(new Rect(lPad, currHeight, position.width / 2 - rPad, 25),
                "Spawn at Pos")) SpawnInWorld(spwnPos);
        currHeight += 30;
        if (GUI.Button(new Rect(lPad, currHeight, position.width - rPad, 25),
                "Spawn in world (Crosshair)"))
        {
            var camT = Camera.main.transform;
            if (camT != null)
                if (Physics.Raycast(camT.position, camT.forward, out var hit))
                    SpawnInWorld(hit.point);
        }

        currHeight += 30;
    }

    private void OnValidate()
    {
        triedWhileNotPlayMode = false;
    }


    private void SpawnInInventory()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Can only spawn during RunTime");
            triedWhileNotPlayMode = true;
            return;
        }

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
        if (!Application.isPlaying)
        {
            Debug.LogError("Can only spawn during RunTime");
            triedWhileNotPlayMode = true;
            return;
        }

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