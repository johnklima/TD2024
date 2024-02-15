using UnityEngine;

public class UIInventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public bool AddItem(BaseItem baseItem)
    {
        foreach (var slot in inventorySlots)
        {
            var itemInSlot = slot.GetComponentInChildren<DraggableItem>();
            if (itemInSlot == null ||
                itemInSlot.item != baseItem ||
                itemInSlot.count >= 10 ||
                itemInSlot.item.stackable != true) continue;

            itemInSlot.count++;
            itemInSlot.RefreshCount();
            return true;
        }

        foreach (var slot in inventorySlots)
        {
            var itemInSlot = slot.GetComponentInChildren<DraggableItem>();
            if (itemInSlot != null) continue;
            SpawnNewItem(baseItem, slot);
            return true;
        }

        return false;
    }

    private void SpawnNewItem(BaseItem item, InventorySlot slot)
    {
        var newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        var inventoryItem = newItemGo.GetComponent<DraggableItem>();
        inventoryItem.InitialiseItem(item);
    }
}