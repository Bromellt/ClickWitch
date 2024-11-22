using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float damage;
    public float maxHP;
    public float level;
    public float currentHP;
    public HealthBar healthBar;

    private void Start() 
    {
        healthBar = FindObjectOfType<HealthBar>();
    }

    public bool TakeDamage(float damage)
    {
        currentHP -= damage;
        healthBar.SetHealth(currentHP);
        if(currentHP <= 0)
        {
            return true;
        }
        else return false;
    }

}
