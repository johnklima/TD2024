using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/item")]
public class ItemUI : ScriptableObject
{

    [Header("Only gameplay")]

    public ItemType type;

    [Header("Only UI")]
    public bool stackable = true;
    public Sprite image;

    public enum ItemType
    {
        IngredientUI, PotionUI
    }

}



















