using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class InventoryChangeEvent: UnityEvent<List<InventoryItem>, bool> {}


public class InventoryController : MonoBehaviour
{
    [SerializeField] private int inventorySlots = 9;
    [SerializeField] private int stackSize = 10;
    
    public InventoryChangeEvent onInventoryChanged = new InventoryChangeEvent();
    
    private readonly List<InventoryItem> _inventoryItems = new();

    private Dictionary<BaseItem, int> _inv = new();

    public InventoryChangeEvent onInventoryChanged = new();


    public void AddItem(BaseItem interactableItem)
    {
        // if (_inv.ContainsKey(interactableItem))
        // {
        //     _inv[interactableItem]++;
        // }
        // else _inv[interactableItem] = 1;
        // onInventoryChanged.Invoke(_inventoryItems, itemAdded);
        // foreach (var keyValuePair in _inv)  
        // {  
        //     Console.WriteLine("Key: {0}, Value: {1}", keyValuePair.Key, keyValuePair.Value);
        //     if (interactableItem == keyValuePair.Key)
        //     {
        //     }
        // }


        var itemAdded = false;
        var shouldAdd = true;
        foreach (var item in _inventoryItems)
            if (item.IsSameItemType(new InventoryItem(interactableItem)))
            {
                if (item.quantity < stackSize)
                {
                    Debug.Log("added another item to stack");

                    item.quantity++;
                    itemAdded = true;
                    break;
                }
                
            }
            if (!itemAdded && _inventoryItems.Count < inventorySlots && shouldAdd)
            {
                Debug.Log("added new thing");

                Debug.Log("too big stack");
                shouldAdd = false;
                break;
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