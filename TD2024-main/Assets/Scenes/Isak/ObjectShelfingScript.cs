using System;
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
     *  
     *  
     *  Consider using a Lerp for the movement of the item.
     *  https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html
     *  
     */

    [SerializeField] private LayerMask layerMask; //We have yet to make an Item layer. -- Set a Default layer when available
    private bool occupied = false;
    private bool itemSettled = false;
    [SerializeField] private float minimumDistance = 0.02f, incrementalMovement = 0.95f;    // minimum distance should be tiny, and incremental movement should be less than 1f
    private GameObject currentlyHeldItem;
    




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layerMask) //If the hit box collides with an item
        {
            if (!occupied)  //check If the spot is avalible for a new Item
            {
                currentlyHeldItem = other.gameObject;   //Set a reference to the item stored
                PlaceItem();
            }
        }
    }

    private void PlaceItem()    // Move the item into place with an updating position, lock the item.
    {
        occupied = true;
        currentlyHeldItem.transform.parent = gameObject.transform;
        currentlyHeldItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    



    public void Reset()     //Undoes everything that has been applied to the item, 
    {
        currentlyHeldItem.transform.parent = null;
        currentlyHeldItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        currentlyHeldItem = null;
        itemSettled = false;
    }

    private void UpdateItemPosition()
    {
        if (currentlyHeldItem.transform.localPosition.z + currentlyHeldItem.transform.localPosition.x + currentlyHeldItem.transform.localPosition.y > minimumDistance)    //If the local z+x+y is more than minimumDistance (0.02f standard) then increment the local position closer to local 0,0,0
        {
            currentlyHeldItem.transform.localPosition = currentlyHeldItem.transform.localPosition * incrementalMovement;  //incrementally moves the item closer to the center.
            //Debug.Log("Moved towards the Center");
        }
        else
        {
            currentlyHeldItem.transform.localPosition = new Vector3(0, 0, 0);       // snaps the item in place and stops the loop.
            itemSettled = true;
            //Debug.Log("Reached the end");
            return;
        }
    }

    private void Update()       //needen to use update so the code would not happen instantly
    {
        if(currentlyHeldItem != null && !itemSettled)
        {
            UpdateItemPosition();
        }
            
    }

}
