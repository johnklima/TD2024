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
    public Matches matches;
    private SphereCollider _sphereCollider;

    private void Awake()
    {
        Register();
        _sphereCollider = GetComponent<SphereCollider>();
        // _sphereCollider.isTrigger = true;
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

    public bool MatchesWith(Matches itemMatches)
    {
        //var item1 = slotA.item;
        //var item2 = slotB.item;
        //var ingredient1 = item1.itemData.ingredient;
        //var ingredient2 = item2.itemData.ingredient;  
        // foreach potion
        // if item1.item.itemData.ingredient == potion.itemData.ingredient1
        // or if item1 == potion.itemData.ingredient2
        // or if item2 == potion.itemData.ingredient1
        // or if item2 == potion.itemData.ingredient2  
        // if item1.MatchesWith(item2)

        // if ()

        return false;
    }

    // public GameObject GetActiveGameObject()
    // {
    //     foreach (var itemManagerItem in _itemManager.inventoryController.objects)
    //     {
    //         var item = itemManagerItem.GetComponent<Item>();
    //         if (item.itemData == itemData)
    //             return itemManagerItem;
    //     }
    //
    //     return null;
    // }


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
    //TODO MatchesWith()
    [Serializable]
    public class Matches
    {
        public bool blue, red, skull, sun, whirlpool, yellow;
    }
}