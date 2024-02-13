
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject inventorySlots;
    private Dictionary<BaseItem, InventorySlot> itemToSlotMap = new Dictionary<BaseItem, InventorySlot>();

    public GameObject defaultCrossHair;
    public GameObject interactiveCrossHair;
    
    private bool isShowingInteractiveCrosshair = false;

    //inventory logic
    public void UpdateSlot(BaseItem item, int quantity)
    {
        Debug.Log("Going in here");
        if (itemToSlotMap.TryGetValue(item, out InventorySlot slot))
        {
            Debug.Log("updating quantity");

            slot.UpdateQuantity(quantity);
        }
        else
        {
            Debug.Log("adding new item");

            InventorySlot emptySlot = FindEmptySlot();
            if (emptySlot != null)
            {
                Debug.Log("had empty slot, adding more");

                emptySlot.SetItem(item, quantity);
                itemToSlotMap[item] = emptySlot;
            }
        }
    }

    private InventorySlot FindEmptySlot()
    {
        foreach (Transform slotTransform in inventorySlots.transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            if (slot != null && slot.Item == null)
            {
                return slot;
            }
        }

        return null;
    }
    //crosshair logic
    public void ToggleCrosshair()
    { 
        isShowingInteractiveCrosshair = !isShowingInteractiveCrosshair;
        
        defaultCrossHair.SetActive(!isShowingInteractiveCrosshair);
        interactiveCrossHair.SetActive(isShowingInteractiveCrosshair);
        
        //Debug.Log($"Interactive Crosshair now {(isShowingInteractiveCrosshair ? "shown" : "hidden")}");

    }
    
}

