using UnityEngine;
using UnityEngine.EventSystems;

public class ViewportMouseHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private AutoScrollCredits _autoScrollCredits;

    private void Start()
    {
        _autoScrollCredits = GetComponentInParent<AutoScrollCredits>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _autoScrollCredits.StopScroll();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _autoScrollCredits.StartScroll();
    }
}