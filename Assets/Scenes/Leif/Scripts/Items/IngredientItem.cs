using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Make Items/Ingredient", order = 2, fileName = "New Ingredient")]
public class IngredientItem : BaseItem
{
    public Ingredient ingredient;
    [SerializeField] private Ingredient[] matches = new Ingredient[6];


    public bool Match(Ingredient a)
    {
        if (matches.Length == 0)
            throw new Exception("No matches set for Ingredient: " + name);
        Debug.Log("matches: " + matches);
        Debug.Log("Ingredient a: " + a);
        return matches.Contains(a);
    }
}