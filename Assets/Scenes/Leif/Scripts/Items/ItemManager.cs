using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemManager : MonoBehaviour
{
    private static ItemManager _instance;
    public static List<Item> items = new();
    public UnityEvent<BaseItem> onItemInteract;
    public int itemCount;

    public static ItemManager instance
    {
        get
        {
            if (_instance != null) return _instance;
            var newManager = new GameObject("-- ItemManager --");
            _instance = newManager.AddComponent<ItemManager>();
            return _instance;
        }
        private set => _instance = value;
    }


    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this);
        else _instance = this;
    }

    private void OnValidate()
    {
        itemCount = items.Count;
    }


    public ItemManager Register(Item item)
    {
        if (!items.Contains(item))
            items.Add(item);
        return _instance;
    }

    public void DeRegister(Item item)
    {
        items.Remove(item);
    }
}