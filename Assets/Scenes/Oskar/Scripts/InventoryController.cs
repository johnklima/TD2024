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
        if (_inventory.ContainsKey(interactableItem))
        {
            if (_inventory[interactableItem] < stackSize)
                _inventory[interactableItem]++;
        }
        else if (_inventory.Count < inventorySlots)
        {
            _inventory[interactableItem] = 1;
        }

        onInventoryChanged.Invoke(_inventory);
        //Show UI/make sound to show that its full
    }

    public void RemoveItem(Item interactableItem) // attaches to ThrowingHandler.OnThrowing()
    {
        Debug.Log("RemoveItem interactableItem: " + interactableItem);
        if (_inventory.ContainsKey(interactableItem))
        {
            if (_inventory[interactableItem] > 0)
                _inventory[interactableItem]--;
            if (_inventory[interactableItem] == 0)
                //TODO: remove from hotBar
                Debug.Log("TODO: remove from hotbar");
        }

        onInventoryChanged.Invoke(_inventory);
        //Show UI/make sound to show that its full
    }
}