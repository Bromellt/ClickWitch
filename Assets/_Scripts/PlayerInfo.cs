using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInfo : MonoBehaviour
{
    //stats
    public float maxHP;
    public float currentHP;
    public int currentMana = 0;
    public int maxMana = 6;

    //health UI
    public HealthBar healthBar;

    //mana ui
    public Image[] manaSlots; // Array of the 6 mana slot images
    public Sprite fullManaSprite; // Sprite for a full mana slot
    public Sprite emptyManaSprite; // Sprite for an empty mana slot
    

    //shield
    public bool shieldActive;
    public float shieldStrength;
    public GameObject shieldPrefab;
    public Transform shieldInstantiatePos;


    private void Start() 
    {
        healthBar = FindObjectOfType<HealthBar>();
        UpdateManaUI();
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

    public void GainMana(int amount)
    {
        currentMana = Mathf.Clamp(currentMana + amount, 0, maxMana);
        UpdateManaUI();
    }

    public void GainSheild()
    {
        //instantiate shield icon
        shieldPrefab = Instantiate(shieldPrefab, shieldInstantiatePos);
    }


    //UI stuff

    // Updates the mana slots' visuals
    public void UpdateManaUI()
    {
        for (int i = 0; i < manaSlots.Length; i++)
        {
            if (i < currentMana)
            {
                manaSlots[i].sprite = fullManaSprite; // Slot is full
            }
            else
            {
                manaSlots[i].sprite = emptyManaSprite; // Slot is empty
            }
        }
    }
}
