using UnityEngine;

public class DemoScriptItemSpawn : MonoBehaviour
{
    public UIInventoryManager UIInventoryManager;
    public ItemUI[] itemsToPickUp;

    public void PickupItem(int id)
    {
        // bool result = UIInventoryManager.AddItem(itemsToPickUp[id]);
        // if (result == true)
        // {
        //     Debug.Log("ItemAdded");
        // }
        // else
        // {
        //     Debug.Log("ItemNOTAdded");
        // }
    }
}