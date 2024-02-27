using UnityEngine;

[CreateAssetMenu(menuName = "Make Items/Potion", order = 1, fileName = "New Potion")]
public class PotionItem : BaseItem
{
    public Potion potion;
    public Ingredient ingredient1;
    public Ingredient ingredient2;
}