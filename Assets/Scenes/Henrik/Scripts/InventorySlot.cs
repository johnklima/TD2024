using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            var dropped = eventData.pointerDrag;
            var draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.parentAfterDrag = transform;
        }
    }
}