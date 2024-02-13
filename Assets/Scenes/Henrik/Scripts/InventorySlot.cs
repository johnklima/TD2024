using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public bool isCraftSlot;
    public BaseItem Item { get; private set; }
    public Image slotImage;
    public Text quantityText;
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

    public void SetItem(BaseItem item, int quantity)
    {
        Item = item;
        slotImage.sprite = item.uiSprite;
        slotImage.enabled = true;
        if (quantityText != null)
        {
            quantityText.text = quantity.ToString();

        }
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (quantityText != null)
        {
            quantityText.text = newQuantity.ToString();

        }
    }
}