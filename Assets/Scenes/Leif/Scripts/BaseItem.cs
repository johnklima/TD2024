using System;
using UnityEngine;

public enum IngredientColor
{
    Red,
    Yellow,
    Blue
}

public enum IngredientSymbol
{
    Skull,
    Sun,
    Whirlpool
}

public enum ItemType
{
    Ingredient,
    Potion
}

[Serializable]
public class ItemSettings
{
    public ItemType itemType;
    public IngredientColor ingredientColor;
    public IngredientSymbol ingredientSymbol;
}

[Serializable]
public class ObjectSettings
{
    public Sprite uiSprite;
    public GameObject prefab;
    public Material material;
}

public abstract class BaseItem : ScriptableObject
{
    public ItemSettings itemSettings;
    public ObjectSettings objectSettings;

    public abstract void Interact();
}