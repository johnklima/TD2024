using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public BaseItem itemData;

    private void Awake()
    {
        ItemManager.instance.Register(this);
    }

    private void OnDestroy()
    {
        ItemManager.instance.DeRegister(this);
    }

    private void OnValidate()
    {
        if (itemData != null) gameObject.name = itemData.name;
    }

    public void Interact()
    {
        ItemManager.instance.onItemInteract?.Invoke(itemData);
    }

    public void Interact(LeifPlayerController lPC)
    {
        ItemManager.instance.onItemInteract?.Invoke(itemData);
    }
}