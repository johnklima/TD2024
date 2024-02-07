using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    //TBD:
    //Inventory UI Handler thing

    public int inventorySlots = 9;
    private List<InventoryItem> _inventoryItems = new List<InventoryItem>();
    public int stackSize = 10;

    

    public void AddItem(InteractableItem interactableItem)
    {
        if (_inventoryItems != null)
        {
            foreach (var item in _inventoryItems)
            {
                Debug.Log(item);

                if (item.IsSameItemType(new InventoryItem(interactableItem.itemType, interactableItem.ingredientType)) && item.quantity < stackSize)
                {
                    Debug.Log("added things of the same type");
                    item.quantity++;
                    return;
                }
                
            }
        }


        if (_inventoryItems.Count < inventorySlots)
        {
            Debug.Log("added new thing");

            _inventoryItems.Add(new InventoryItem(interactableItem.itemType, interactableItem.ingredientType));

        }
       
        else
        {
            //Show UI/make sound to show that its full
            Debug.Log("inventory is full");
        }
    }
}

public class InventoryItem
{
    public InteractableItem.InteractableItemType itemType;
    public InteractableItem.IngredientType ingredientType;

    public int quantity;

    public InventoryItem(InteractableItem.InteractableItemType itemType, InteractableItem.IngredientType ingredientType,
        int quantity = 1)
    {
        this.itemType = itemType;
        this.ingredientType = ingredientType;
        this.quantity = quantity;
    }
    public bool IsSameItemType(InventoryItem other)
    {
        return ingredientType == other.ingredientType && itemType == other.itemType;
    }
}
