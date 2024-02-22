using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private int inventorySlots = 9;
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
        foreach (var itemManagerItem in objects)
            if (itemManagerItem.TryGetComponent<Item>(out var item))
                if (item.itemData == interactableItem.itemData)
                    return item;
        return null;
    }

    // listens to OnItemInteract @ ItemManager
    public void AddItem(Item interactableItem)
    {
        Debug.Log("AddItem interactableItem: " + interactableItem);
        interactableItem = GetActiveItemInstance(interactableItem);
        if (_inventory.ContainsKey(interactableItem))
        {
            var stackable = interactableItem.itemData.stackable;
            if ((stackable && _inventory[interactableItem] < stackSize) ||
                (!stackable && _inventory[interactableItem] < 1))
                _inventory[interactableItem]++;
        }
        // else if (_inventory.Count < inventorySlots)
        // {
        //     _inventory[interactableItem] = 1;
        // }

        onInventoryChanged.Invoke(_inventory);
        //Show UI/make sound to show that its full
    }

    public void RemoveItem(DraggableItem draggableItem) //! attaches to ThrowingHandler.OnThrowing()
    {
        var item = draggableItem.item;
        if (!_inventory.ContainsKey(item)) return;
        if (_inventory[item] > 0) _inventory[item]--;
        //Show UI/make sound to show that its full
        onInventoryChanged.Invoke(_inventory);
    }
}