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

public enum Potion
{
    RedSkull,
    RedSun,
    RedWhirlpool,
    RedRed,
    YellowSkull,
    YellowSun,
    YellowWhirlpool,
    YellowYellow,
    BlueSkull,
    BlueSun,
    BlueWhirlpool,
    BlueBlue
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
    public bool stackable;
}