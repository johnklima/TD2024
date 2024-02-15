using System;
using Unity.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(SphereCollider))]
public class Item : MonoBehaviour, IInteractable
{
    public BaseItem itemData;
    [ReadOnly] public int id;
    [SerializeField] private ItemManager _itemManager;
    public bool deactivateOnInteract;

    private void Awake()
    {
        Register();
    }

    private void OnDisable()
    {
        Register();
    }

    private void OnDrawGizmos()
    {
        Handles.Label(transform.position + transform.up * 0.25f, $"{itemData.name}\n{itemData.itemType}");
    }


    private void OnValidate()
    {
        Register();
    }

    public void Interact()
    {
        _itemManager.onItemInteract?.Invoke(itemData);
        gameObject.SetActive(deactivateOnInteract);
    }

    public void Interact(LeifPlayerController lPC)
    {
        _itemManager.onItemInteract?.Invoke(itemData);
        gameObject.SetActive(deactivateOnInteract);
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
}