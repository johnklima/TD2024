using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Make Items/Ingredient", order = 2, fileName = "New Ingredient")]
public class IngredientItem : BaseItem
{
    public Ingredient ingredient;
}