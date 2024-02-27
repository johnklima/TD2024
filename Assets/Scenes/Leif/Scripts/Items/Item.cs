using System;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(SphereCollider))]
public class Item : MonoBehaviour, IInteractable
{
    //TODO get item to collide
    public BaseItem itemData;
    [SerializeField] private ItemManager _itemManager;
    public bool isInteractionOneShot;
    public UnityEvent<Collision, Item> onCollision;


    [HideInInspector] public int id;
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

    private void OnCollisionEnter(Collision other)
    {
        var isPLayer = other.gameObject.CompareTag("Player");
        if (isPLayer) return;
        if (!isInteractionOneShot) return;
        onCollision.Invoke(other, this);
        Destroy(gameObject);
    }


    protected virtual void OnValidate()
    {
        Register();
    }

    public void Interact()
    {
        _itemManager.onItemInteract?.Invoke(this);
        gameObject.SetActive(!isInteractionOneShot);
    }

    public void Interact(LeifPlayerController lPC)
    {
        _itemManager.onItemInteract?.Invoke(this);
        gameObject.SetActive(!isInteractionOneShot);
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
    private void OnDrawGizmosSelected()
    {
        Handles.Label(transform.position + transform.up * 0.25f, $"{itemData.name}\n{itemData.itemType}");
    }

    public void ShowGizmos()
    {
        Handles.Label(transform.position + transform.up * 0.25f, $"{itemData.name}\n{itemData.itemType}");
    }
#endif
}