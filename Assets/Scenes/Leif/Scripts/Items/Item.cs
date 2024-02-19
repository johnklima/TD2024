using System;
using Unity.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(SphereCollider))]
public class Item : MonoBehaviour, IInteractable
{
    //TODO get item to collide
    public BaseItem itemData;
    [ReadOnly] public int id;
    [SerializeField] private ItemManager _itemManager;
    public bool isOneShot;
    private SphereCollider _sphereCollider;

    private void Awake()
    {
        Register();
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.isTrigger = true;
    }

    private void OnDisable()
    {
        Register();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.Label(transform.position + transform.up * 0.25f, $"{itemData.name}\n{itemData.itemType}");
    }
#endif

    private void OnValidate()
    {
        Register();
    }

    public void Interact()
    {
        _itemManager.onItemInteract?.Invoke(this);
        gameObject.SetActive(!isOneShot);
    }

    public void Interact(LeifPlayerController lPC)
    {
        _itemManager.onItemInteract?.Invoke(this);
        gameObject.SetActive(!isOneShot);
    }

    public GameObject GetActiveGameObject()
    {
        foreach (var itemManagerItem in _itemManager.items)
            if (itemManagerItem.itemData == itemData)
                return itemManagerItem.gameObject;
        return null;
    }

    public Item GetActiveItemInstance()
    {
        foreach (var itemManagerItem in _itemManager.items)
            if (itemManagerItem.itemData == itemData)
                return itemManagerItem;
        return null;
    }

    private void Register()
    {
        if (!isActiveAndEnabled) return;
        ValidateItemManager();
        _itemManager.Register();
    }

    private void ValidateItemManager()
    {
        if (_itemManager != null) return;
        var existingManager = FindObjectOfType<ItemManager>();
        if (existingManager != null && existingManager.isActiveAndEnabled)
        {
            _itemManager = existingManager;
        }
        else
        {
            var newManager = new GameObject("-- ItemManager --");
            _itemManager = newManager.AddComponent<ItemManager>();
            if (_itemManager == null) throw new Exception("No item manager found: Contact Leif");
        }
    }


#if UNITY_EDITOR
    public void ShowGizmos()
    {
        Handles.Label(transform.position + transform.up * 0.25f, $"{itemData.name}\n{itemData.itemType}");
    }
#endif
}