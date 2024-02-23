using Unity.VisualScripting;
using UnityEngine;

public class IngredientObjectItem : Item
{
    [DoNotSerialize] public IngredientItem itemData2;

    public Color color1, color2;

    public void CopyData()
    {
        var asd = GetComponent<Item>();
        if (asd == null) return;
        itemData2 = (IngredientItem)asd.itemData;
        itemData = (IngredientItem)asd.itemData;
        DestroyImmediate(asd, true);
    }
}