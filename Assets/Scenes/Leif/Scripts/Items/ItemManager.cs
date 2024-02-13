using UnityEngine;
using UnityEngine.Events;

public class ItemManager : MonoBehaviour
{
    public Item[] items;
    public UnityEvent<BaseItem> onItemInteract;

    private void Awake()
    {
        RegisterItems();
    }

    private void OnValidate()
    {
        RegisterItems();
    }

    public void TestItemInteract(BaseItem baseItem)
    {
        Debug.Log($"Interaction on item: {baseItem.name}");
    }

    private void RegisterItems()
    {
        if (!isActiveAndEnabled) return;
        items = FindObjectsByType<Item>(FindObjectsSortMode.None);
        for (var i = 0; i < items.Length; i++) items[i].id = i;
    }


    public void Register(Item item)
    {
        RegisterItems();
    }
}