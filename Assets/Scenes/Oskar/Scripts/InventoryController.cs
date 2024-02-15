using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private int inventorySlots = 9;
    [SerializeField] private int stackSize = 10;

    public UnityEvent<Dictionary<Item, int>> onInventoryChanged = new();


    private readonly Dictionary<Item, int> _inventory = new();

    // listens to OnItemInteract @ ItemManager
    public void AddItem(Item interactableItem)
    {
        var itemAdded = false;
        Debug.Log("interactableItem: " + interactableItem);
        if (_inventory.ContainsKey(interactableItem))
        {
            if (_inventory[interactableItem] < stackSize)
            {
                _inventory[interactableItem]++;
                itemAdded = true;
            }
        }
        else if (!itemAdded && _inventory.Count < inventorySlots)
        {
            _inventory[interactableItem] = 1;
            itemAdded = true;
        }


        onInventoryChanged.Invoke(_inventory);
        //Show UI/make sound to show that its full
    }
}