using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int playerhealth;
    public int numberOfHearts;

    public Image[] hearts;
    public Sprite fullheart;
    public Sprite nullheart;

    private void Update()
    {
for (int i = 0; i < hearts.Length; i++)
        {
            if(i < numberOfHearts)
            {
                hearts[i].enabled = false;
          
            }
        }
    }
}