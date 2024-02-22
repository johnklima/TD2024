using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;
    public bool isCraftSlot;
    private InventoryController _inventoryController;

    public void Awake()
    {
        _inventoryController = FindObjectOfType<InventoryController>();
        if (_inventoryController == null) throw new Exception("Make sure there is an InventoryController in the scene");
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

            //TODO can we slot it?
            //
            // if (!isCraftSlot) return;
            // // if we are a crafting slot
            // // if more than 1 item, return to previous parent, reduce inv
            // if (draggableItem.count > 1)
            //     draggableItem.transform.SetParent(draggableItem.prevParent);
            // // else if only 1 item, delete UI element, reduce inv
            // else if (draggableItem.count == 1) //? do we need to care if its less than 1?
            //     Destroy(draggableItem.GameObject());
            //
            // // reduce inventory
            // _inventoryController.RemoveItem(draggableItem);
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