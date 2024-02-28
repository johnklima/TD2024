using System;
using UnityEngine;
using UnityEngine.Events;

public class ItemManager : MonoBehaviour
{
    public Item[] items;

    // public UnityEvent<BaseItem> onItemInteract;
    public UnityEvent<Item> onItemInteract;
    public InventoryController inventoryController;

    public AudioClip itemBreakAudioClip, itemHealAudioClip;
    public bool showItemGizmos;
    private AudioSource _audioSource;

    private void Awake()
    {
        RegisterItems();
        inventoryController = FindObjectOfType<InventoryController>();
        if (inventoryController == null) throw new Exception("No inventoryController found");


        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;


        if (itemBreakAudioClip == null ||
            itemHealAudioClip == null)
            throw new Exception("No audio clip for items");
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!showItemGizmos) return;
        for (var i = 0; i < items.Length; i++) items[i].ShowGizmos();
    }
#endif

    private void OnValidate()
    {
        RegisterItems();
    }

    public GameObject GetActiveGameObject(Item checkItem)
    {
        foreach (var invItem in inventoryController.objects)
        {
            var item = invItem.GetComponent<Item>();
            if (item.itemData == checkItem.itemData)
                return invItem;
        }

        return null;
    }


    public void TestItemInteract(BaseItem baseItem)
    {
        Debug.Log($"Interaction on item: {baseItem.name}");
    }

    private void RegisterItems()
    {
        if (!isActiveAndEnabled) return;
        items = FindObjectsByType<Item>(FindObjectsSortMode.None);
        for (var i = 0; i < items.Length; i++)
        {
            items[i].id = i;
            items[i].onCollision.AddListener(PlayItemSound);
        }
    }

    private void PlayItemSound(Collision arg0, Item arg1)
    {
        //itemHealAudioClip itemBreakAudioClip
        if (arg0.gameObject.CompareTag("Enemies"))
            _audioSource.clip = itemHealAudioClip;
        else
            _audioSource.clip = itemBreakAudioClip;
        _audioSource.Play();
    }


    public void Register()
    {
        RegisterItems();
    }
}