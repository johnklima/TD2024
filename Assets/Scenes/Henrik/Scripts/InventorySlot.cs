using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public bool isCraftSlot;

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            var dropped = eventData.pointerDrag;
            var draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.parentAfterDrag = transform;
            if (isCraftSlot)
            {
                // check if recipe is good, behave accordingly
            }
        }
    }
}