using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private int maxInvSlots = 9, currNumUsedSlots;
    [SerializeField] private int stackSize = 10;

    public GameObject[] objects;

    public UnityEvent<Dictionary<Item, int>> onInventoryChanged = new();

    private readonly Dictionary<Item, int> _inventory = new();

    private void Start()
    {
        foreach (var o in objects)
        {
            var i = o.GetComponent<Item>();
            _inventory[i] = 0;
        }
    }

    public void TestOnInventoryChanged()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Can only test during RunTime");
            return;
        }

        onInventoryChanged.Invoke(_inventory);
    }

    public Item GetActiveItemInstance(Item interactableItem)
    {
        foreach (var _item in objects)
            if (_item.TryGetComponent<Item>(out var item))
                if (item.itemData == interactableItem.itemData)
                    return item;
        return null;
    }

    public Item GetPotionFromIngredients(Ingredient a, Ingredient b)
    {
        foreach (var _item in objects)
            if (_item.TryGetComponent<Item>(out var item))
            {
                if (item.itemData.itemType == ItemType.Ingredient) continue;
                var potion = item.GetComponent<PotionObjectItem>();
                var i1 = potion.itemData2.ingredient1;
                var i2 = potion.itemData2.ingredient2;
                Debug.Log($"i1: {i1} - i2: {i2}");
                Debug.Log($"i1: {i1} - i2: {i2}");
                Debug.Log($"(i1 == a && i2 == b: {i1 == a && i2 == b}");
                Debug.Log($"(i1 == b && i2 == a): {i1 == b && i2 == a}");
                if ((i1 == a && i2 == b) || (i1 == b && i2 == a))
                    return item;
            }

        return null;
    }

    // listens to OnItemInteract @ ItemManager
    public void AddItem(Item interactableItem)
    {
        if (currNumUsedSlots == maxInvSlots) return;
        interactableItem = GetActiveItemInstance(interactableItem);
        if (_inventory.ContainsKey(interactableItem))
        {
            var stackable = interactableItem.itemData.stackable;
            if ((stackable && _inventory[interactableItem] < stackSize) ||
                (!stackable && _inventory[interactableItem] < 1))
            {
                _inventory[interactableItem]++;
                if (_inventory[interactableItem] == 1)
                    currNumUsedSlots++;
            }
        }

        Debug.Log("inv : " + _inventory);
        onInventoryChanged.Invoke(_inventory);
        //Show UI/make sound to show that its full
    }

    public void RefreshUIInventory()
    {
        onInventoryChanged.Invoke(_inventory);
    }

    public void RemoveItem(DraggableItem draggableItem) //! attaches to ThrowingHandler.OnThrowing()
    {
        var item = GetActiveItemInstance(draggableItem.item);
        if (!_inventory.ContainsKey(item)) throw new Exception("inventory does not contain: " + draggableItem.name);
        if (_inventory[item] > 0)
        {
            _inventory[item] -= 1;
            if (_inventory[item] == 0)
                currNumUsedSlots -= 1;
        }

        //Show UI/make sound to show that its full
        onInventoryChanged.Invoke(_inventory);
    }
}