using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private int inventorySlots = 9;
    [SerializeField] private int stackSize = 10;


    public UnityEvent<Dictionary<Item, int>> onInventoryChanged = new();

    private readonly Dictionary<Item, int> _inventory = new();

    public void TestOnInventoryChanged()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Can only test during RunTime");
            return;
        }

        onInventoryChanged.Invoke(_inventory);
    }

    // listens to OnItemInteract @ ItemManager
    public void AddItem(Item interactableItem)
    {
        Debug.Log("AddItem interactableItem: " + interactableItem);

        interactableItem = interactableItem.GetActiveItemInstance();
        if (_inventory.ContainsKey(interactableItem))
        {
            var stackable = interactableItem.itemData.stackable;
            if ((stackable && _inventory[interactableItem] < stackSize) ||
                (!stackable && _inventory[interactableItem] < 1))
                _inventory[interactableItem]++;
        }
        else if (_inventory.Count < inventorySlots)
        {
            _inventory[interactableItem] = 1;
        }

        onInventoryChanged.Invoke(_inventory);
        //Show UI/make sound to show that its full
    }

    public void RemoveItem(DraggableItem draggableItem) //! attaches to ThrowingHandler.OnThrowing()
    {
        var item = draggableItem.item;
        if (!_inventory.ContainsKey(item)) return;
        if (_inventory[item] > 0) _inventory[item]--;
        if (_inventory[item] == 0) _inventory.Remove(item);
        //Show UI/make sound to show that its full
        onInventoryChanged.Invoke(_inventory);
    }
}