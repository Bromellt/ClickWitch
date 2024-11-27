using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;


public enum SpellType
{
    Fire,
    Ice,
    Shadow,
    Heal
}

public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST} // might add another state for typing
public class BattleSystem : MonoBehaviour
{
    //gameobjects for tracking
    public BattleState state;
    public GameObject enemyPrefab;
    public Transform enemyLocation;
    Enemy enemy;
    public List<Enemy> allEnemies = new List<Enemy>();
    public TextMeshProUGUI dialogueText; 
    public BattleHUD enemyHUD;
    public GameObject SpellbookHUD;
    public GameObject sizeIndicatorPrefab;
    public Transform indicatorInstantiatePos;
    private GameObject activeIndicator;

    // fields for typing minigame
    public WordManager wordManager;
    public WordSpawner wordSpawner;
    private SpellType currentSpellType;

    // UI Buttons for spell selection
    public Button spell1;
    public Button spell2;
    public Button spell3;
    public Button spell4;


    //track player and spells
    PlayerInfo playerInfo;
    Spell currentSpell;
    public Spellbook playerSpellbook;
    private List<Spell> currentSpells; //current 4 spells from the spellbook
 
    void Start()
    {
        playerInfo = FindAnyObjectByType<PlayerInfo>();
        SetupBattle();
    }

    void SetupBattle()
    {
        state = BattleState.START;

        /*
        //choose random enemy
        int randomIndex = Random.Range(0, allEnemies.Count);
        enemy = allEnemies[randomIndex];
        */

        GameObject enemyGO = Instantiate(enemyPrefab, enemyLocation);
        enemy = enemyGO.GetComponent<Enemy>(); //get info from enemy

        StartCoroutine(ShowDialogue("A " + enemy.enemyName + " approaches", .05f, 1f, () => PlayerTurn()));

        enemyHUD.SetHUD(enemy);


        

    }
    public void PlayerTurn()
    {
        state = BattleState.PLAYERTURN;
        Debug.Log("got to player turn");
        //gain 1 mana at the start of every turn
        playerInfo.GainMana(1);

        //setup HUD info
        playerSpellbook.GenerateTurnSpells();
        currentSpells = playerSpellbook.currentTurnSpells;

        // Update the spell buttons dynamically
        PopulateSpellButtons();

        StartCoroutine(ShowDialogue("Your turn! Choose your spell.", 0.05f, .25f, () => SetBattleHUD())); // Enable spell selection
        

    }

    //function called on button press to open the spell options
    public void SetBattleHUD()
    {
        if(state!= BattleState.PLAYERTURN)
            return;
        
        SpellbookHUD.SetActive(true);
    }

     void PopulateSpellButtons()
    {
        // Assign spells to each button
        if (currentSpells.Count > 0) SetButton(spell1, currentSpells[0]);
        if (currentSpells.Count > 1) SetButton(spell2, currentSpells[1]);
        if (currentSpells.Count > 2) SetButton(spell3, currentSpells[2]);
        if (currentSpells.Count > 3) SetButton(spell4, currentSpells[3]);
    }

    //assigns spells to buttons and checks if the player can cast it
    void SetButton(Button button, Spell spell)
    {
        button.gameObject.SetActive(true); // Ensure button is visible
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = $"{spell.spellName}\nCost: {spell.manaCost} Mana";

        // Check if the player has enough mana to cast the spell
        if (playerInfo.currentMana >= spell.manaCost)
        {
            button.interactable = true; // Enable button if enough mana
            button.onClick.RemoveAllListeners(); // Clear old listeners
            button.onClick.AddListener(() => SelectSpell(spell)); // Assign new listener
        }
        else
        {
            button.interactable = false; // Disable button if not enough mana
        }
    }

    //on button press when selecting which spell to cast
    //also calls the start spell cast function that starts the typing minigame
    public void SelectSpell(Spell spell)
    {
        currentSpell = spell;
        if (playerInfo.currentMana >= spell.manaCost)
        {
            playerInfo.GainMana(-spell.manaCost); // Deduct mana
            playerInfo.UpdateManaUI(); //update mana ui

            Debug.Log($"Selected spell: {spell.spellName}. Remaining Mana: {playerInfo.currentMana}");
            currentSpellType = spell.type;
            StartSpellCast(spell);
        }
        else
        {
            Debug.Log("Not enough mana to cast this spell!");
        }
    }
    
    //calls wordmanager to start the typing minigame
    public void StartSpellCast(Spell spell)
    {
        activeIndicator = Instantiate(sizeIndicatorPrefab, indicatorInstantiatePos);
        wordManager.spellIndicator = activeIndicator.GetComponent<IndicatorControl>();

        wordSpawner.currentSpellState = SpellState.Active;
        wordManager.AddWord(spell);  // Start a new word typing sequence
        SpellbookHUD.SetActive(false);  // Hide the spellbook HUD after selecting a spell
    }


    void EnemyTurn()
    {
        state = BattleState.ENEMYTURN;
        
        Debug.Log("Enemy's turn!");
        
        
        //logic

        //delays for animations if needed
        //show dialogue
        //StartCoroutine(ShowDialogue());

        PlayerTurn();
    }


    void EndBattle()
    {
        state = BattleState.WON;

        //show ui maybe gain new spell
    }


    void LoseBattle()
    {
        state = BattleState.LOST;
    }



    public float CalculateDamage(int totalWords, int wordsTyped, float deducations, Spell spell)
    {
        // Base damage scales with the percentage of words typed correctly
        float accuracy = (float)(totalWords - deducations) / totalWords; // Accuracy as a percentage

        float finalDamage = spell.baseDamage+wordsTyped * accuracy;
        return Mathf.Max(0, finalDamage); // Ensure damage isn't negative
    }

    public float CalculateShield(Spell spell, int wordsTyped, int totalWords, float deducations)
    {

        float accuracy = (float)(totalWords - deducations) / totalWords; // Accuracy as a percentage

        return spell.shieldStrength+wordsTyped * accuracy;

    }

    //called after typing minigame to damage enemy or give shield
    public void ResolveSpellEffect(int wordsTyped)
    {

        //maybe later have animation of spell firing from the indicator

        // Remove the active indicator (only the clone)
        if (activeIndicator != null)
        {
            Destroy(activeIndicator);
        }
        activeIndicator = null; // Clear the reference

        //calculate values
        int totalWords = wordManager.wordsMissed+wordsTyped;
        float deductions = .5f*wordManager.mistakes + wordManager.wordsMissed; //missed whole words worth 2x missed letter


        

        //if spell is a shield
        if (currentSpell.shieldStrength > 0f)
        {
            // Apply shield effect
            playerInfo.shieldActive = true;
            playerInfo.GainSheild();


            playerInfo.shieldStrength = CalculateShield(currentSpell, wordsTyped, totalWords, deductions);
            StartCoroutine(ShowDialogue("Player cast " + currentSpell.spellName + " and gained a shield blocking " + currentSpell.shieldStrength * 100 + "% of the next attack!"));

        }

        //if spell is damaging
        if(currentSpell.baseDamage>0f)
        {
            float spellStrength = CalculateDamage(totalWords, wordsTyped, deductions, currentSpell);

            if(currentSpell.hitCount > 1)
            {
                for (int i = 0; i < currentSpell.hitCount; i++)
                {
                    bool isDead = enemy.TakeDamage(spellStrength);
                    if (isDead)
                    {
                        EndBattle();
                    }
                }
                EnemyTurn();
                StartCoroutine(ShowDialogue(currentSpell.spellName + " hits with " + spellStrength + " damage" + currentSpell.hitCount + " times."));
            }
            else
            {
                bool isDead = enemy.TakeDamage(spellStrength);
                if (isDead)
                {
                    EndBattle();
                }
                else
                {
                    EnemyTurn();
                    StartCoroutine(ShowDialogue("player casts " + currentSpell.spellName + " for " + spellStrength + " damage."));
                }
                    
            }
            
        }
        else
        {
            EnemyTurn();
        }
             

    }

    //gives defualt for certain perameters but also the option to use them
    IEnumerator ShowDialogue(string message, float typingSpeed = 0.05f, float displayTime = 1.5f, Action onDialogueComplete = null)
    {
        // Clear current text
        dialogueText.text = "";

        // Typing effect: display one character at a time
        foreach (char letter in message)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait for additional time after the full message is displayed
        yield return new WaitForSeconds(displayTime);

        // Clear the dialogue text again
        dialogueText.text = "";

        // Call the callback action to show player options
        onDialogueComplete?.Invoke();

    }

    
}
