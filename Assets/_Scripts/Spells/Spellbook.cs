using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellbook : MonoBehaviour
{

    public List<Spell> allSpells = new List<Spell>(); // The player's full spellbook
    public List<Spell> currentTurnSpells = new List<Spell>(); // Spells selected for the current turn
    private int spellsPerTurn = 4;

    public void GenerateTurnSpells()
    {
        currentTurnSpells.Clear();

        // Shuffle and pick 4 random spells from the spellbook
        List<Spell> shuffledSpells = new List<Spell>(allSpells);
        for (int i = 0; i < spellsPerTurn && shuffledSpells.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, shuffledSpells.Count);
            currentTurnSpells.Add(shuffledSpells[randomIndex]);
            shuffledSpells.RemoveAt(randomIndex);
        }

        // Notify UI to update (add a delegate or event here if needed)
        UpdateSpellUI();
    }


    private void UpdateSpellUI()
    {
        Debug.Log("Update spell UI with new turn spells");
        foreach (Spell spell in currentTurnSpells)
        {
            Debug.Log($"Spell: {spell.spellName}");
        }
    }

    // Method to add a new spell to the spellbook
    public void AddSpell(Spell newSpell)
    {
        allSpells.Add(newSpell);
    }
}
