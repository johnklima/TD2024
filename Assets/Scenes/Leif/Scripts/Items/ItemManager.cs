﻿using UnityEngine;
using UnityEngine.Events;

public class ItemManager : MonoBehaviour
{
    public Item[] items;

    // public UnityEvent<BaseItem> onItemInteract;
    public UnityEvent<Item> onItemInteract;

    public bool showItemGizmos;

    private void Awake()
    {
        RegisterItems();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!showItemGizmos) return;
        for (var i = 0; i < items.Length; i++) items[i].ShowGizmos();
    }
#endif

    private void OnValidate()
    {
        RegisterItems();
    }


    public void TestItemInteract(BaseItem baseItem)
    {
        Debug.Log($"Interaction on item: {baseItem.name}");
    }

    private void RegisterItems()
    {
        if (!isActiveAndEnabled) return;
        items = FindObjectsByType<Item>(FindObjectsSortMode.None);
        for (var i = 0; i < items.Length; i++) items[i].id = i;
    }


    public void Register()
    {
        RegisterItems();
    }
}