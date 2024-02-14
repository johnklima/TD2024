using UnityEngine;

public class DemoScriptItemSpawn : MonoBehaviour
{
    public UIInventoryManager UIInventoryManager;
    public ItemUI[] itemsToPickUp;

    public void PickupItem(int id)
    {
        UIInventoryManager.AddItem(itemsToPickUp[id]);

    }




}