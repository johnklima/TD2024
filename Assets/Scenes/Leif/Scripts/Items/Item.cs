using System;
using Unity.Collections;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public BaseItem itemData;
    public GameObject itemManager;
    [ReadOnly] public int id;
    private ItemManager _itemManager;

    private void Awake()
    {
        ValidateItemManager();
        Register();
    }

    private void OnDisable()
    {
        Debug.Log("VAR");
        Register();
    }


    private void OnValidate()
    {
        ValidateItemManager();
        Register();
    }

    public void Interact()
    {
        _itemManager.onItemInteract?.Invoke(itemData);
    }

    public void Interact(LeifPlayerController lPC)
    {
        _itemManager.onItemInteract?.Invoke(itemData);
    }

    private void Register()
    {
        _itemManager.Register(this);
    }

    private void ValidateItemManager()
    {
        if (_itemManager == null)
        {
            _itemManager = FindObjectOfType<ItemManager>();
            if (_itemManager == null)
            {
                var newManager = Instantiate(itemManager);
                _itemManager = newManager.GetComponent<ItemManager>();
                if (_itemManager == null) throw new Exception("No item manager found: Contact Leif");
            }
        }
    }
}