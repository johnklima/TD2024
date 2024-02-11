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
    
    public InventoryChangeEvent onInventoryChanged = new InventoryChangeEvent();
    
    
    private readonly Dictionary<BaseItem, int>_inventory = new ();
    
    public void AddItem(BaseItem interactableItem)
    {
        bool itemAdded = false;
        
        if (_inventory.ContainsKey(interactableItem))
        {
            Debug.Log(interactableItem);
            if (_inventory[interactableItem] < stackSize)
            {
                _inventory[interactableItem]++;
                itemAdded = true;
            }
        }
        else if(_inventory.Count < inventorySlots && !itemAdded)
        {
            _inventory[interactableItem] = 1;
            itemAdded = true;
        }
        
       
            //Show UI/make sound to show that its full
    }
}
