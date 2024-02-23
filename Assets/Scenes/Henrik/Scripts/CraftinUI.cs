using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftinUI : MonoBehaviour
{
    public GameObject craftingSlot;
    public Button mixButton;
    private InventoryController _inventoryController;
    private List<IngredientItem> currentIngredients, prevIngredients;

    private bool ingredientsMatch;

    private void Awake()
    {
        _inventoryController = FindObjectOfType<InventoryController>();
        if (_inventoryController == null) throw new Exception("Make sure there is an InventoryController in the scene");
        mixButton.onClick.AddListener(OnMix);
        craftingSlot.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (_inventoryController == null)
            _inventoryController = FindObjectOfType<InventoryController>();

        currentIngredients = new List<IngredientItem>();

        mixButton.interactable = false;
        craftingSlot.SetActive(true);
        CursorLockHandler.ShowAndUnlockCursor();
    }

    private void OnDisable()
    {
        craftingSlot.SetActive(false);
        CursorLockHandler.HideAndLockCursor();
    }

    public void OnCraftingSlotDrop(DraggableItem draggableItem)
    {
        var item = _inventoryController.GetActiveItemInstance(draggableItem.item);
        var ingredientObjectItem = item.gameObject.GetComponent<IngredientObjectItem>();
        var iItem = ingredientObjectItem.itemData2;
        if (iItem == null)
            throw new Exception("did not find matching item in inventoryController.objects for: " + draggableItem.name);

        if (currentIngredients.Count < 2)
            currentIngredients.Add(iItem);
        mixButton.interactable = currentIngredients.Count == 2;
        if (currentIngredients.Count == 2)
        {
            // we have 2 ingredients. make mix button active.

            //todo make button active
            // check ingredients match

            var ingItem1 = currentIngredients[0];
            var ingItem2 = currentIngredients[1];
            ingredientsMatch = ingItem1.Match(ingItem2.ingredient);

            // empty list for next try
            prevIngredients = currentIngredients;
            currentIngredients.Clear();
        }

        // we have gotten X items
        Destroy(draggableItem.gameObject);
        _inventoryController.RemoveItem(draggableItem);
    }

    private void PotionFactory(Ingredient iItem1, Ingredient iItem2)
    {
        Debug.Log(" making potion"); // making potion
        var potionItem = _inventoryController.GetPotionFromIngredients(iItem1, iItem2);
        if (potionItem == null) return;
        _inventoryController.AddItem(potionItem);
    }


    public void OnMix()
    {
        mixButton.interactable = false;

        if (ingredientsMatch)
        {
            Debug.Log(" make potion"); // make potion
            PotionFactory(prevIngredients[0].ingredient, prevIngredients[1].ingredient);
        }
        else
        {
            Debug.Log(" make poof"); // make poof
        }
    }
}