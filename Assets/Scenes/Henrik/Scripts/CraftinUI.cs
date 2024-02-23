using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftinUI : MonoBehaviour
{
    public GameObject craftingSlot;
    public Button mixButton;

    [SerializeField] private MeshRenderer cauldronSoupRenderer;
    private InventoryController _inventoryController;
    private Material cauldronSoupMaterial;
    private List<IngredientItem> currentIngredients = new(), prevIngredients = new();

    private bool ingredientsMatch;

    private void Awake()
    {
        if (cauldronSoupRenderer == null)
            throw new Exception("Make sure there is an MeshRenderer set on cauldronSoupRenderer");
        cauldronSoupMaterial = cauldronSoupRenderer.sharedMaterial;

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

    private void SetCauldronSoupColor(IngredientObjectItem item)
    {
        Debug.Log("Setting soup color: " + item.color1);
        cauldronSoupMaterial.SetColor("_BaseColor", item.color1);
        cauldronSoupMaterial.SetColor("_VoronoyColor", item.color2);
    }

    public void OnCraftingSlotDrop(DraggableItem draggableItem)
    {
        var item = _inventoryController.GetActiveItemInstance(draggableItem.item);
        var ingredientObjectItem = item.gameObject.GetComponent<IngredientObjectItem>();
        var iItem = ingredientObjectItem.itemData2;
        if (iItem == null)
            throw new Exception("did not find matching item in inventoryController.objects for: " + draggableItem.name);

        if (currentIngredients.Count < 2)
        {
            Debug.Log("Slotted first item: " + draggableItem.name);
            currentIngredients.Add(iItem);
        }

        mixButton.interactable = currentIngredients.Count == 2;
        if (currentIngredients.Count < 2) // it was first item, find color and set shader
            SetCauldronSoupColor(ingredientObjectItem);
        if (currentIngredients.Count == 2)
        {
            // we have 2 ingredients. make mix button active.
            Debug.Log("Slotted second item: " + draggableItem.name);

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
        Debug.Log("Removing UI element, updating inventory");
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
            Debug.Log(" make potion //TODO TODO TESTING"); // make potion
            Debug.Log(" prevIngredients[0]: " + prevIngredients[0]); // make potion
            Debug.Log(" prevIngredients[1]: " + prevIngredients[1]); // make potion
            Debug.Log(" ingredient[0]: " + prevIngredients[0].ingredient); // make potion
            Debug.Log(" ingredient[1]: " + prevIngredients[1].ingredient); // make potion
            Debug.Log("<color=red><size=20>CLICK ME; READ ME</size></color>"); // make potion
            //TODO you are trying to figure out why you got a ArgumentOutOfRangeException on next line
            // you tried initializing prevIngredients, but not tested.
            PotionFactory(prevIngredients[0].ingredient, prevIngredients[1].ingredient);
        }
        else
        {
            Debug.Log(" make poof"); // make poof
        }
    }
}