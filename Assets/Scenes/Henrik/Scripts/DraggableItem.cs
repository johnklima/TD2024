using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Text countText;

    [HideInInspector] public Item item;

    // [HideInInspector] public ItemUI item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;


    [Header("UI")] private Image image;

    public void Start()
    {
        InitialiseItem(item);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        if (image == null) image = GetComponent<Image>();
        if (image == null) throw new Exception("GameObject must have Image component!");
        // image.sprite = newItem.image;2
        image.sprite = newItem.itemData.uiSprite;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        var textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }
}