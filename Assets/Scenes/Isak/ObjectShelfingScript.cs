using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShelfingScript : MonoBehaviour
{
    /*  ver. 0.1
     *  creation date: 23.01.2024
     *  Last Update: 23.01.2024
     *  
     *  This script is made with the intention of having items thrown into specific spaces to be placed 
     *  in snaping positions on things like a shelf.
     * 
     *  I plan on making this a script for a invisible box area, where if an Item touches the box, 
     *  it is placed inside with a small animation to align it right place. with only one item per box.
     */

    [SerializeField] private LayerMask layerMask; //We have yet to make an Item layer. -- Set a Default layer when available
    private bool occupied = false;




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layerMask) //If the box collides with an item
        {
            if (!occupied)  //check If the spot is avalible for a new Item
            {
                PlaceItem();
            }
        }
    }

    private void PlaceItem()
    {
        Debug.Log("Reached the end");   // Move the item into place with an animation, stop gravity and lock the item. remember to set as occupied.
    }



}
