using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthSystem : MonoBehaviour
{
    private int maxHp = 3;
    private int currentHp = 3;
    public UnityEvent UpdateHealthUI = new();
    public UnityEvent Die = new();
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        //Just some paranoia
        if (currentHp < 0)
        {
            currentHp = 0;
        }

        UpdateHealthUI.Invoke();
        if (currentHp <= 0)
        {
            Die.Invoke();
        }
    }

    public void Heal(int healAmount)
    {
        currentHp += healAmount;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        UpdateHealthUI.Invoke();
    }

    //Currently used to update the UI in the HealthTest Script, might be useful for the actual UI.
    public int GetCurrentHp()
    {
        return currentHp;
    }
    
    //If the player's current hp is equal to the max hp, then the player shouldn't be able to heal themself.
    //This disallows wasting healing items.
    public bool CanHeal()
    {
        return currentHp < maxHp;
    }

}
