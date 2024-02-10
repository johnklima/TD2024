using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class InventoryChangeEvent : UnityEvent<List<InventoryItem>, bool>
{
}


public class InventoryController : MonoBehaviour
{
    [SerializeField] private int inventorySlots = 9;
    [SerializeField] private int stackSize = 10;

    public InventoryChangeEvent onInventoryChanged = new();

    public UnityEvent<Dictionary<BaseItem, int>> onItemPickUp = new(); //! leif
    private readonly Dictionary<BaseItem, int> _inv = new(); //! leif
    private readonly List<InventoryItem> _inventoryItems = new();

    public void TestAddItemEvent(Dictionary<BaseItem, int> inv) //! leif code starts here
    {
        Debug.Log("inventory changed: ");
        foreach (var kVP in inv) Debug.Log($"Item: {kVP.Key} has: {kVP.Value} in the stack");
    } //! leif code ends here

    public void AddItem(BaseItem interactableItem)
    {
        //! leif code starts here
        if (_inv.ContainsKey(interactableItem)) // if we have item
        {
            if (_inv[interactableItem] < stackSize) // and count is less that stacksize
                _inv[interactableItem]++; //increment stack
            //else show error, stack full or whatever 
        }
        else
        {
            _inv[interactableItem] = 1; // if we dont have item, initialize new with value of 1
        }

        onItemPickUp.Invoke(_inv); // invoke event for UI
        //! leif code ends here
        var itemAdded = false;
        var shouldAdd = true;
        foreach (var item in _inventoryItems)
            if (item.IsSameItemType(new InventoryItem(interactableItem)))
                if (item.quantity < stackSize)
                {
                    Debug.Log("added another item to stack");

                    item.quantity++;
                    itemAdded = true;
                    break;
                }

        if (!itemAdded && _inventoryItems.Count < inventorySlots && shouldAdd)
        {
            Debug.Log("added new thing");

            Debug.Log("too big stack");
            shouldAdd = false;
        }

        if (!itemAdded && _inventoryItems.Count < inventorySlots && shouldAdd)
        {
            Debug.Log("added new thing");

            _inventoryItems.Add(new InventoryItem(interactableItem));
            itemAdded = true;
        }

        if (!itemAdded)
            //Show UI/make sound to show that its full
            Debug.Log("inventory is full");
        onInventoryChanged.Invoke(_inventoryItems, itemAdded);
    }
}

public class InventoryItem
{
    public BaseItem baseItem;

    public int quantity;

    public InventoryItem(BaseItem baseItem, int quantity = 1)
    {
        this.quantity = quantity;
        this.baseItem = baseItem;
    }

    public bool IsSameItemType(InventoryItem other)
    {
        if (baseItem.GetType() == other.baseItem.GetType())
        {
            if (baseItem is IngredientItem ingredientItem && other.baseItem is IngredientItem otherIngredient)
                return ingredientItem.ingredient == otherIngredient.ingredient;

            if (baseItem is PotionItem potionItem && other.baseItem is PotionItem otherPotionItem)
                return potionItem.Ingredients.ingredient1 == otherPotionItem.Ingredients.ingredient1 &&
                       potionItem.Ingredients.ingredient2 == otherPotionItem.Ingredients.ingredient2;
        }

        return false;
    }
}