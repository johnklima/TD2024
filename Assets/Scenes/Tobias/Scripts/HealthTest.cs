using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTest : MonoBehaviour
{
    //This is only a test scrip.
    //This only works with the max health of 3.
    
    [SerializeField] private PlayerHealthSystem playerHealth;
    [SerializeField] private GameObject[] hearts;


    public void LooseHealth()
    {
        
    }
    
    public void GainHealth()
    {
        
    }

    public void UpdateHealthUI()
    {
        //Not the best way to do this, but it's a test.
        int hp = playerHealth.GetCurrentHp();
        Debug.Log(hp);
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < hp)
            {
                hearts[i].SetActive(true);
            }
            else
            {
                hearts[i].SetActive(false);
            }
        }
    }
}
