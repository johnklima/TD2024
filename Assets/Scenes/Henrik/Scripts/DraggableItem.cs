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
    [Header("UI")] private Image image;

    public void Start()
    {
        InitialiseItem(item, count);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        prevParent = parentAfterDrag = transform.parent;
        transform.SetParent(root);
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

    public void InitialiseItem(Item newItem, int count)
    {
        item = newItem;
        if (image == null) image = GetComponent<Image>();
        if (image == null) throw new Exception("GameObject must have Image component!");
        image.sprite = item.itemData.uiSprite;
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