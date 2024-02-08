using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryChangeEvent: UnityEvent<List<InventoryItem>, bool> {}


public class InventoryController : MonoBehaviour
{
    
    [SerializeField] private int inventorySlots = 9;
    private List<InventoryItem> _inventoryItems = new List<InventoryItem>();
    [SerializeField] private int stackSize = 10;

    public InventoryChangeEvent onInventoryChanged = new InventoryChangeEvent();


    public void AddItem(BaseItem interactableItem)
    {
        bool itemAdded = false;
        
            foreach (var item in _inventoryItems)
            {
                if (item.IsSameItemType(new InventoryItem(interactableItem)) && item.quantity < stackSize)
                {
                    Debug.Log("added another item to stack");

                    item.quantity++;
                    itemAdded = true;
                    break;
                }
                
            }
            if (!itemAdded && _inventoryItems.Count < inventorySlots)
            {
                Debug.Log("added new thing");

                _inventoryItems.Add(new InventoryItem(interactableItem, 1 ));
                itemAdded = true;

            }
       
            if(!itemAdded)
            {
                //Show UI/make sound to show that its full
                Debug.Log("inventory is full");
            }
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
            if(baseItem is IngredientItem ingredientItem && other.baseItem is IngredientItem otherIngredient )
            {
                return ingredientItem.ingredient == otherIngredient.ingredient;
            }

            if (baseItem is PotionItem potionItem && other.baseItem is PotionItem otherPotionItem)
            {
                return potionItem.Ingredients.ingredient1 == otherPotionItem.Ingredients.ingredient1 &&
                       potionItem.Ingredients.ingredient2 == otherPotionItem.Ingredients.ingredient2;
            } 
        }

        return false;

    }
}
