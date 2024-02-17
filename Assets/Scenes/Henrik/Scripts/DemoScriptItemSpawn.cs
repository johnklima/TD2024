using UnityEngine;
using UnityEngine.Serialization;

public class DemoScriptItemSpawn : MonoBehaviour
{
    [FormerlySerializedAs("UIInventoryManager")]
    public InventoryDisplay inventoryDisplay;

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