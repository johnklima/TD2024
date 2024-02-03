using UnityEngine;

public enum Ingredient
{
    Red,
    Yellow,
    Blue,
    Skull,
    Sun,
    Whirlpool
}

public enum ItemType
{
    Ingredient,
    Potion
}


public abstract class BaseItem : ScriptableObject
{
    public Sprite uiSprite;
    public ItemType itemType;

    public abstract void Interact();
}