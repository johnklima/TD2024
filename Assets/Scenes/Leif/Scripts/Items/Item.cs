using System;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Item : MonoBehaviour, IInteractable
{
    public BaseItem itemData;
    [ReadOnly] public int id;
    [SerializeField] private ItemManager _itemManager;

    private void Awake()
    {
        Register();
    }

    private void OnDisable()
    {
        Register();
    }


    private void OnValidate()
    {
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
        if (!isActiveAndEnabled) return;
        ValidateItemManager();
        _itemManager.Register();
    }

    private void ValidateItemManager()
    {
        if (_itemManager == null)
        {
            var existingManager = FindObjectOfType<ItemManager>();
            Debug.Log(existingManager);
            if (existingManager != null && existingManager.isActiveAndEnabled)
            {
                Debug.Log(existingManager);
                _itemManager = existingManager;
            }
            else

            {
                var newManager = new GameObject("-- ItemManager --");
                _itemManager = newManager.AddComponent<ItemManager>();
                if (_itemManager == null) throw new Exception("No item manager found: Contact Leif");
            }
        }
    }
}