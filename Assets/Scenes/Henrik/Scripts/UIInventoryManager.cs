using System;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    [HideInInspector] public Item selectedItem;
    private int selectedSlot = -1;

    public void Start()
    {
        ChangeSelectedSlot(0);
    }

    public void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            var newSlot = selectedSlot + 1;
            if (newSlot > inventorySlots.Length - 1) newSlot = 0;
            ChangeSelectedSlot(newSlot);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            var newSlot = selectedSlot - 1;
            if (newSlot < 0) newSlot = 8;
            ChangeSelectedSlot(newSlot);
        }
    }

    private void OnValidate()
    {
        inventorySlots = GetComponentsInChildren<InventorySlot>();
    }

    private void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0) inventorySlots[selectedSlot].Deselect();
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
        var slotTransform = inventorySlots[newValue].transform;
        if (slotTransform.childCount > 0)
        {
            var draggable = slotTransform.GetComponentInChildren<DraggableItem>();
            selectedItem = draggable.item;
        }
        else
        {
            selectedItem = null;
        }

        Debug.Log("selectedItem: " + selectedItem);
    }

    //TODO listen to OnInventoryChanged @ InventoryController

    public bool TryUpdateItemInSlot(DraggableItem slot, KeyValuePair<Item, int> item)
    {
        if (!slot.item.itemData.stackable || slot.count >= 10) return false;
        if (slot.item.itemData != item.Key.itemData) return false;
        slot.count = item.Value;
        slot.RefreshCount();
        return true;
    }

    private DraggableItem FindDraggableItemInSlot(KeyValuePair<Item, int> item)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.transform.childCount == 0) continue;
            var itemInSlot = slot.GetComponentInChildren<DraggableItem>();
            if (itemInSlot.item.itemData == item.Key.itemData) return itemInSlot;
        }

        return null;
    }

    public void AddItem(Dictionary<Item, int> inventory)
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
            var stackable = item.Key.itemData.stackable
                ? "stackable, stack-size: " + draggableItem.count
                : "un-stackable";
            if (!TryUpdateItemInSlot(draggableItem, item))
                msg = $"Failed to update: ${item.Key}! {stackable}";
            Debug.Log(msg);
        }
    }

    private bool FindAndPopulateEmptySlot(KeyValuePair<Item, int> item)
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

    private void SpawnNewItem(Item item, InventorySlot slot)
    {
        var newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        var inventoryItem = newItemGo.GetComponent<DraggableItem>();
        inventoryItem.InitialiseItem(item);
    }
}