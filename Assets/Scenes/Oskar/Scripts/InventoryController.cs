using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InventoryController : MonoBehaviour
{
    [SerializeField] private int inventorySlots = 9;
    [SerializeField] private int stackSize = 10;
    
    public UnityEvent<BaseItem,int> onInventoryChanged = new ();
    
    
    private readonly Dictionary<BaseItem, int>_inventory = new ();
    
    public void AddItem(BaseItem interactableItem)
    {
        Debug.Log("inventory controller");
        bool itemAdded = false;
        Debug.Log(interactableItem);

        if (_inventory.ContainsKey(interactableItem))
        {
            Debug.Log(interactableItem);
            if (_inventory[interactableItem] < stackSize)
            {
                _inventory[interactableItem]++;
                itemAdded = true;
            }
        }
        else if(!itemAdded && _inventory.Count < inventorySlots)
        {
            _inventory[interactableItem] = 1;
            itemAdded = true;
        }
        
        onInventoryChanged.Invoke(interactableItem, _inventory[interactableItem]);
        //Show UI/make sound to show that its full
    }
}
