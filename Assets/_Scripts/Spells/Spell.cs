using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class Spell : ScriptableObject 
{
	public string spellName;
	public SpellType type; // Enum for Fire, Water, Shadow
	public int manaCost;
	public int baseDamage;
    public int hitCount = 1; // Number of hits (default to 1)
	public float shieldStrength = 0f; // Shield percentage (0 for non-shielding spells)
	public string[] wordPool; // Words used for this spell
	//public StatusEffect effect; // Enum for Burn, Slow, etc.


	public string GetRandomWord()
	{
		int randomIndex = Random.Range(0, wordPool.Length);
        string randomWord = wordPool[randomIndex];
        return randomWord;
	}
}

