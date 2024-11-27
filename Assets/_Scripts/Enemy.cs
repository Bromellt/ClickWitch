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

    public bool shieldActive;
    public float shieldStrength = 0f; // Percentage of damage to block (e.g., 0.5 for 50%)
    public GameObject shieldPrefab;
    public Transform shieldInstantiatePos;

    private void Start() 
    {
        healthBar = FindObjectOfType<HealthBar>();
    }

    public bool TakeDamage(float damage)
    {
        if(shieldActive)
        {
            //calc new damage value
            int effectiveDamage = Mathf.RoundToInt(damage * (1f - shieldStrength)); // Reduce damage based on shield

            //destroy active shield
            shieldActive = false;
            shieldStrength = 0f;
            if(shieldPrefab != null)
            {
                Destroy(shieldPrefab);
            }

            //take damage
            currentHP -= effectiveDamage;
            
        }
        else
        {
            currentHP -= damage;
        }

        
        healthBar.SetHealth(currentHP);
        if(currentHP <= 0)
        {
            return true;
        }
        else return false;
    }

    public void GainSheild()
    {
        //instantiate shield icon
        shieldPrefab = Instantiate(shieldPrefab, shieldInstantiatePos);
    }

}
