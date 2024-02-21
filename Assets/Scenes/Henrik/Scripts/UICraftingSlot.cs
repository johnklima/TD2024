using UnityEngine;
using UnityEngine.EventSystems;

public class UICraftingSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        /* on drop, update inventory, update ui.
         * update caldron shader
         * 
         * 
         * 
         * 
         * 
         */
        if (transform.childCount == 0)
        {
            var dropped = eventData.pointerDrag;
            var draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.parentAfterDrag = transform;
            draggableItem.gameObject.SetActive(false);
        }
    }

    private void Start()
    {

    }
}
