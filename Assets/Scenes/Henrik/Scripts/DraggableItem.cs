using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Text countText;
    public Transform root;
    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Transform prevParent;

    private InventoryDisplay _inventoryDisplay;
    [Header("UI")] private Image image;

    private Image symbolImage;

    public void Start()
    {
        InitialiseItem(item, count);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        prevParent = parentAfterDrag = transform.parent;
        transform.SetParent(root);
        _inventoryDisplay.OnStartDrag();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        _inventoryDisplay.OnEndDrag();
    }

    public void InitialiseItem(Item newItem, int count, InventoryDisplay inventoryDisplay = null)
    {
        if (inventoryDisplay != null)
            _inventoryDisplay = inventoryDisplay;
        item = newItem;
        if (image == null) image = GetComponent<Image>();
        if (image == null) throw new Exception("GameObject must have Image component!");
        image.sprite = item.itemData.uiSprite;

        if (newItem.itemData.itemType == ItemType.Potion)
        {
            var potion = newItem.gameObject.GetComponent<PotionObjectItem>();
            // if potion get symbol
            if (symbolImage == null) symbolImage = GetComponentInChildren<Image>();
            if (symbolImage == null) throw new Exception("GameObject must have Image component!");
            // set symbol
            symbolImage.sprite = potion.itemData2.uiSpriteSymbol;
            symbolImage.enabled = true;
        }

        this.count = count;
        root = GetComponentInParent<Canvas>().transform;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        var textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }
}