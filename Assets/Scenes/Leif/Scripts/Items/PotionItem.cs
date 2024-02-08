using System;
using UnityEngine;

[Serializable]
public struct Ingredients
{
    public Ingredient ingredient1;
    public Ingredient ingredient2;
}

[Serializable]
public class ObjectSettings
{
    public GameObject prefab;
    public Material material;
}

[CreateAssetMenu(menuName = "Make Items/Potion", order = 1, fileName = "New Potion")]
public class PotionItem : BaseItem
{
    public Ingredients Ingredients;
    public ObjectSettings objectSettings;
}