using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IHuman : MonoBehaviour
{
    public bool isSelectet;

    public int Health, currentHealth;
    public int Armor;
    public float Speed;
    public int Damage;
    public float attackSpeed;
    public Color typeColor;


    public bool isDead()
    {
        if(currentHealth <= 0)
        {
            return true;
        }
        return false;
    }

}
