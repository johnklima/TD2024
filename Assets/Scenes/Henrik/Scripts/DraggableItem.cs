using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{


    [Header("UI")]
    public Image image;
    [HideInInspector] public ItemUI item;

    [HideInInspector] public Transform parentAfterDrag;

    public void Start()
    {
        image = (Image)GetComponent(typeof(Image));
        InitialiseItem(item);

    }
    public void InitialiseItem(ItemUI newItem)
    {

        image.sprite = newItem.image;

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
}