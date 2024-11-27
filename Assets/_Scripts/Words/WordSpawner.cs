using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;


public enum SpellState
{
    Active,  // Spell is active, words are being spawned
    Inactive // Spell has ended, no words should spawn
}

public class WordSpawner : MonoBehaviour
{
    public GameObject wordPrefab;
    public Transform wordCanvas;




    public WordManager wordManager;
    public float wordDelay = 1.5f;
    public SpellState currentSpellState = SpellState.Active;

    public float spellTimeMax; //total time for the player to type
    public float spellTimer; //current time left
    public TextMeshProUGUI timerText;
    
    private void Start() 
    {
        spellTimer = spellTimeMax;
    }

    private void Update() 
    {

        if (currentSpellState == SpellState.Active)
        {

            spellTimer-=Time.deltaTime;

            // Round the timer to the nearest decimal place and update the text
            float roundedTimer = Mathf.Round(spellTimer * 100f) / 100f;
            timerText.text = roundedTimer.ToString("0.00");

            //stop the timer when it reaches 0
            if (spellTimer <= 0)
            {
                spellTimer = 0;
                timerText.text = "0.00"; // Ensure it displays 0 when it finishes
                EndSpell();  // End the spell and transition to enemy turn
                
            }

        }
        
    }

    // End the spell and notify BattleSystem
    void EndSpell()
    {
        
        currentSpellState = SpellState.Inactive;  // Stop word spawning

        spellTimer = spellTimeMax; //reset timer

        // Calculate spell strength based on words typed
        int wordsTyped = wordManager.GetWordsTyped();

        BattleSystem battleSystem = FindObjectOfType<BattleSystem>();
        battleSystem.ResolveSpellEffect(wordsTyped);
        wordManager.ResetWordCount();

    }

    
    public WordDisplay SpawnWord()
    {

        if (currentSpellState == SpellState.Inactive)
        {
            return null;  // No words should spawn if the spell is inactive
        }

        //Vector3 randomPos = new Vector3(Random.Range(-9f,9f), 2f);

        Vector3 randomPos = new Vector3(0f,0f,0f);    

        GameObject wordObj = Instantiate(wordPrefab, randomPos, Quaternion.identity, wordCanvas);
        WordDisplay wordDisplay = wordObj.GetComponent<WordDisplay>();

        return wordDisplay;
        
    }
}
