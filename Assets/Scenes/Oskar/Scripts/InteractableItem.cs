using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class InteractableItem: MonoBehaviour, I_Interactable
{
    
    public enum InteractableItemType
    {
        Potion, Cauldron, Ingredient
    }
    public enum IngredientType
    {
        Red,Blue, Yellow, Skull, Sun, Whirlpool
    }
    
    public InteractableItemType itemType;

    public IngredientType ingredientType;
    
    

   

    public void Interact(PlayerController playerController)
    {
        if (itemType == InteractableItemType.Potion || itemType == InteractableItemType.Ingredient)
        {
            playerController.inventory.AddItem(this);
        }
        else
        {
            //start UI for crafting?
        }
    }

    
    
}
