using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;
    public bool isCraftSlot;
    private CraftinUI _craftinUI;
    private InventoryController _inventoryController;

    public void Awake()
    {
        _inventoryController = FindObjectOfType<InventoryController>();
        if (_inventoryController == null) throw new Exception("Make sure there is an InventoryController in the scene");

        if (isCraftSlot)
            _craftinUI = FindObjectOfType<CraftinUI>();

        Deselect();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            var dropped = eventData.pointerDrag;
            var draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.parentAfterDrag = transform;
            // when item is dropped here,

            if (isCraftSlot)
            {
                // if we are a crafting slot and have more than 1 item,
                _craftinUI.OnCraftingSlotDrop(draggableItem);
                _inventoryController.RemoveItem(draggableItem);
                Destroy(draggableItem.GameObject());
            }
        }
    }

    public void Select()
    {
        image.color = selectedColor;
    }

    public void Deselect()
    {
        image.color = notSelectedColor;
    }
}