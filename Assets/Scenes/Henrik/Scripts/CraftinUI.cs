using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftinUI : MonoBehaviour
{
    public GameObject craftingSlot;
    public Button mixButton;

    [SerializeField] private MeshRenderer cauldronSoupRenderer;
    private Material _cauldronSoupMaterial;
    private List<IngredientItem> _currentIngredients = new();

    private bool _ingredientsMatch;
    private InventoryController _inventoryController;
    private IngredientItem[] _prevIngredients = new IngredientItem[2];

    private void Awake()
    {
        if (cauldronSoupRenderer == null)
            throw new Exception("Make sure there is an MeshRenderer set on cauldronSoupRenderer");
        _cauldronSoupMaterial = cauldronSoupRenderer.sharedMaterial;

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

        _currentIngredients = new List<IngredientItem>();

        mixButton.interactable = false;
        craftingSlot.SetActive(true);
        CursorLockHandler.ShowAndUnlockCursor();
    }

    private void OnDisable()
    {
        craftingSlot.SetActive(false);
        _currentIngredients.Clear();
        CursorLockHandler.HideAndLockCursor();
    }

    private void SetCauldronSoupColor(IngredientObjectItem item)
    {
        Debug.Log("Setting soup color: " + item.color1);
        _cauldronSoupMaterial.SetColor("_BaseColor", item.color1);
        _cauldronSoupMaterial.SetColor("_VoronoyColor", item.color2);
    }

    public void OnCraftingSlotDrop(DraggableItem draggableItem)
    {
        var item = _inventoryController.GetActiveItemInstance(draggableItem.item);
        var ingredientObjectItem = item.gameObject.GetComponent<IngredientObjectItem>();
        var iItem = ingredientObjectItem.itemData2;
        if (iItem == null)
            throw new Exception("did not find matching item in inventoryController.objects for: " + draggableItem.name);

        Debug.Log("Slotted second item: " + draggableItem.name);
        _currentIngredients.Add(iItem);
        mixButton.interactable = _currentIngredients.Count == 2; //make mix button active if we have 2
        if (_currentIngredients.Count < 2) // it was first item, find color and set shader
            SetCauldronSoupColor(ingredientObjectItem);
        if (_currentIngredients.Count == 2)
        {
            // we have 2 ingredients. 
            // check ingredients match
            var ingItem1 = _currentIngredients[0];
            var ingItem2 = _currentIngredients[1];
            _ingredientsMatch = ingItem1.Match(ingItem2.ingredient);
            Debug.Log("slotted item 2, match: " + _ingredientsMatch);
            // empty list for next try
            _prevIngredients = _currentIngredients.ToArray();
            _currentIngredients.Remove(_currentIngredients[0]);
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
        Debug.Log("onMix ingredientsMatch: " + _ingredientsMatch);
        if (_ingredientsMatch)
            PotionFactory(_prevIngredients[0].ingredient, _prevIngredients[1].ingredient);
        else
            Debug.Log(" make poof"); // make poof
    }
}