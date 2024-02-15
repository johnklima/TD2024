using System;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    private void OnValidate()
    {
        inventorySlots = GetComponentsInChildren<InventorySlot>();
    }

    //TODO listen to OnInventoryChanged @ InventoryController

    public bool TryUpdateItemInSlot(DraggableItem slot, KeyValuePair<BaseItem, int> item)
    {
        if (!slot.item.stackable || slot.count >= 10) return false;
        if (slot.item != item.Key) return false;
        slot.count = item.Value;
        slot.RefreshCount();
        return true;
    }

    private DraggableItem FindDraggableItemInSlot(KeyValuePair<BaseItem, int> item)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.transform.childCount == 0) continue;
            var itemInSlot = slot.GetComponentInChildren<DraggableItem>();
            if (itemInSlot.item == item.Key) return itemInSlot;
        }

        return null;
    }

    public void AddItem(Dictionary<BaseItem, int> inventory)
    {
        //foreach (var keyValuePair in inventory) Debug.Log($"{keyValuePair.Key}:{keyValuePair.Value}");
        foreach (var item in inventory)
        {
            // scan UI to find and update elements
            var draggableItem = FindDraggableItemInSlot(item);
            if (draggableItem == null)
            {
                // if we dont find item, we need to make UI element
                var success = FindAndPopulateEmptySlot(item);
                if (success) continue; // then continue to next item;
                throw new Exception("Inventory full??");
            }

            // if we have UI version of item, try to update UI
            var msg = $"Updated: ${item.Key}!";
            var stackable = item.Key.stackable ? "stackable, stack-size: " + draggableItem.count : "un-stackable";
            if (!TryUpdateItemInSlot(draggableItem, item))
                msg = $"Failed to update: ${item.Key}! {stackable}";
            Debug.Log(msg);
        }
    }

    private bool FindAndPopulateEmptySlot(KeyValuePair<BaseItem, int> item)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.transform.childCount != 0) continue;
            // find empty slot
            // make child
            SpawnNewItem(item.Key, slot);
            return true;
        }

        return false;
    }

    private void SpawnNewItem(BaseItem item, InventorySlot slot)
    {
        var newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        var inventoryItem = newItemGo.GetComponent<DraggableItem>();
        inventoryItem.InitialiseItem(item);
    }
}