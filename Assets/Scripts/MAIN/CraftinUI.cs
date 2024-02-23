using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CraftinUI : MonoBehaviour
{
    public GameObject craftingSlot;
    public Button mixButton;

    public UnityEvent onMixSuccess = new(), onMixFail = new();
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
        if (!item.gameObject.TryGetComponent(out IngredientObjectItem ingredientObjectItem))
            throw new Exception("did not find matching item in IngredientObjectItem for: " + draggableItem.name);

        var iItem = ingredientObjectItem.itemData2;
        if (iItem == null)
            throw new Exception("did not find matching item in ingredientObjectItem.itemData2 for: " +
                                draggableItem.name);

        Debug.Log("Slotted item: " + draggableItem);
        currentIngredients.Add(iItem);
        mixButton.interactable = currentIngredients.Count == 2; //make mix button active if we have 2
        if (currentIngredients.Count < 2) // it was first item, find color and set shader
            SetCauldronSoupColor(ingredientObjectItem);
        if (currentIngredients.Count == 2)
        {
            // we have 2 ingredients. 
            // check ingredients match
            var ingItem1 = currentIngredients[0];
            var ingItem2 = currentIngredients[1];
            ingredientsMatch = ingItem1.Match(ingItem2.ingredient);

            // empty list for next try
            prevIngredients = new List<IngredientItem>(currentIngredients);
            currentIngredients.Remove(currentIngredients[0]);
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
            Debug.Log(" make potion miniGame " +
                      "<color=red><size=large>TODO</size></color> " +
                      "make potion miniGame"); // make potion
            //todo make potion miniGame
            PotionFactory(prevIngredients[0].ingredient, prevIngredients[1].ingredient);
            onMixSuccess.Invoke();
        }
        else
        {
            Debug.Log(" make poof"); // make poof
            onMixFail.Invoke();
        }
    }
}