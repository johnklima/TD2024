using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//! AddItem() listens to OnInventoryChanged @ InventoryController
public class InventoryDisplay : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    [HideInInspector] public DraggableItem selectedItem;
    public UnityEvent onStartDrag, onEndDrag;
    private HUD _hud;
    private InventoryController _inventoryController;
    private int selectedSlot = -1;

    public void Start()
    {
        _inventoryController = FindObjectOfType<InventoryController>();
        if (_inventoryController == null) throw new Exception("Make sure there is a _inventoryController in the scene");

        _hud = GetComponentInParent<HUD>();

        ChangeSelectedSlot(0);
    }

    public void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            var newSlot = selectedSlot - 1;
            if (newSlot < 0) newSlot = 8;
            ChangeSelectedSlot(newSlot);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            var newSlot = selectedSlot + 1;
            if (newSlot > inventorySlots.Length - 1) newSlot = 0;
            ChangeSelectedSlot(newSlot);
        }

        PlayDragSound();
    }


    private void OnValidate()
    {
        inventorySlots = GetComponentsInChildren<InventorySlot>();
    }


    public void UpdateInventoryDisplay(Dictionary<Item, int> inventory)
    {
        //* foreach (var keyValuePair in inventory) Debug.Log($"{keyValuePair.Key}:{keyValuePair.Value}");
        foreach (var item in inventory)
        {
            //* if item.value > 0: update or add
            //* scan UI to find and update elements
            //* check if we have UI element for this item
            var draggableItem = FindDraggableItemInSlot(item);
            var stackable = item.Key.itemData.stackable
                ? "stackable, stack-size: " + item.Value
                : "un-stackable";
            if (item.Value > 0)
                //* if we actually have an item
                if (draggableItem == null)
                    //* if we dont have a slot
                {
                    //* if we dont find item, we need to make UI element
                    if (!FindAndPopulateEmptySlot(item)) throw new Exception("Inventory full??"); //* did not make slot
                    //* successfully made slot
                    continue; //* then continue to next item;
                }

            //* if we have UI version of item, try to update UI
            TryUpdateDisplaySlot(draggableItem, item);
        }

        CleanUpInventory(inventory); // remove any unused images
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

    private bool FindAndPopulateEmptySlot(KeyValuePair<Item, int> item)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.transform.childCount != 0) continue;
            //* find empty slot, make child, refresh selectedSlot
            var stackable = item.Key.itemData.stackable
                ? "stackable, stack-size: " + item.Value
                : "un-stackable";
            SpawnNewItem(item, slot);
            ChangeSelectedSlot(selectedSlot);
            return true;
        }

        return false;
    }

    private void SpawnNewItem(KeyValuePair<Item, int> item, InventorySlot slot)
    {
        var newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        var inventoryItem = newItemGo.GetComponent<DraggableItem>();
        inventoryItem.InitialiseItem(item.Key, item.Value, this);
    }

    private void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0) inventorySlots[selectedSlot].Deselect();
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
        var slotTransform = inventorySlots[newValue].transform;
        if (slotTransform.childCount > 0)
            selectedItem = slotTransform.GetComponentInChildren<DraggableItem>();
        else
            selectedItem = null;
    }

    public bool TryUpdateDisplaySlot(DraggableItem slot, KeyValuePair<Item, int> item)
    {
        if (slot == null) return false;
        //* if (slot.item.itemData != item.Key.itemData) return false; // not correct item ?
        //* if (slot.item.itemData.stackable && slot.count >= 10) return false; // stackable but full stack
        //* if (slot.count >= 1) return false; // is not stackable and already have 1

        slot.count = item.Value; // update slot
        slot.RefreshCount(); // update UI element
        return true;
    }

    private void CleanUpInventory(Dictionary<Item, int> inventory)
    {
        foreach (var slot in inventorySlots)
        {
            var dI = slot.GetComponentInChildren<DraggableItem>();
            if (dI == null) continue;
            var item = _inventoryController.GetActiveItemInstance(dI.item);
            if (inventory[item] == 0)
                // if (!inventory.ContainsKey(dI.item))
                Destroy(dI.gameObject);
        }
    }

    public void OnEndDrag()
    {
        PlayDragSound();
        onEndDrag.Invoke();
    }

    public void OnStartDrag()
    {
        PlayDragSound();
        onStartDrag.Invoke();
    }

    private void PlayDragSound()
    {
        _hud.ItemDragged();
    }
}