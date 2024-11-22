using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


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
    public BattleState state;
    public GameObject enemyPrefab;
    public Transform enemyLocation;
    Enemy enemy;
    public TextMeshProUGUI dialogueText; 
    public BattleHUD enemyHUD;
    public GameObject SpellbookHUD;


    // fields for typing minigame
    public WordManager wordManager;
    public WordSpawner wordSpawner;
    private SpellType currentSpellType;

    // UI Buttons for spell selection
    public Button spell1;
    public Button spell2;
    public Button spell3;
    public Button spell4;


    void Start()
    {
        state = BattleState.START;
        SetupBattle();
        SetupSpellButtons();
    }

    void SetupBattle()
    {
        
        GameObject enemyGO = Instantiate(enemyPrefab, enemyLocation);
        enemy = enemyGO.GetComponent<Enemy>(); //get info from enemy

        dialogueText.text = enemy.enemyName + " approaches";

        enemyHUD.SetHUD(enemy);

        state = BattleState.PLAYERTURN;
        PlayerTurn();

    }
    void PlayerTurn()
    {
        //show options hud
        SpellbookHUD.SetActive(true);

    }
    public void OnSpellbookButton()
    {
        if(state!= BattleState.PLAYERTURN)
            return;
        
        SpellbookHUD.SetActive(true);
    }

    // Each of these methods will be linked to a spell selection button in the UI
    public void OnSpell1Selected()
    {
        currentSpellType = SpellType.Fire;  // Set the spell type
        StartSpellCast(currentSpellType);
    }

    public void OnSpell2Selected()
    {
        currentSpellType = SpellType.Ice;   // Set the spell type
        StartSpellCast(currentSpellType);
    }

    public void OnSpell3Selected()
    {
        currentSpellType = SpellType.Shadow; // Set the spell type
        StartSpellCast(currentSpellType);
    }

    public void OnSpell4Selected()
    {
        currentSpellType = SpellType.Heal;   // Set the spell type
        StartSpellCast(currentSpellType);
    }
    

    public void StartSpellCast(SpellType spellType)
    {
        wordManager.spellStrengthCircle.SetActive(true);
        wordSpawner.currentSpellState = SpellState.Active;
        wordManager.AddWord();  // Start a new word typing sequence
        SpellbookHUD.SetActive(false);  // Hide the spellbook HUD after selecting a spell
    }

    // Connect spell buttons to their respective functions
    void SetupSpellButtons()
    {
        spell1.onClick.AddListener(OnSpell1Selected);
        spell2.onClick.AddListener(OnSpell2Selected);
        spell3.onClick.AddListener(OnSpell3Selected);
        spell4.onClick.AddListener(OnSpell4Selected);
    }

    // This function will be called when the player's turn ends
    public void EndPlayerTurn()
    {
        state = BattleState.ENEMYTURN;
        Debug.Log("End of Player's turn. Now it's the enemy's turn!");

        // Here, you would start the enemy turn logic. For now, let's switch to the enemy turn.
        // You can add more logic for enemy actions here.
        StartEnemyTurn();
    }

    void StartEnemyTurn()
    {
        // Simulate the enemy taking their turn. You can add more logic for the enemy's behavior.
        Debug.Log("Enemy's turn!");
        // For example, you could call enemy attack animations or AI decision-making here.

        // After the enemy's turn, you might want to switch back to the player's turn.
        // You can also implement delays or animations as necessary.
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }


    public void ResolveSpellEffect(int wordsTyped)
{
    float spellStrength = wordsTyped * 10f; // Example scaling factor (10 damage per word)
    Debug.Log($"Casting {currentSpellType} with strength: {spellStrength}");

    // Apply spell effects based on type
    switch (currentSpellType)
    {
        case SpellType.Fire:
            spellStrength +=0f;
            dialogueText.text = $"Fire spell hits for {spellStrength} damage!";
            break;
        case SpellType.Ice:
            spellStrength *= .8f;
            dialogueText.text = $"Ice spell chills for {spellStrength * 0.8f} damage!";
            break;
        case SpellType.Shadow:
            spellStrength *= 1.2f;
            dialogueText.text = $"Shadow spell strikes for {spellStrength * 1.2f} damage!";
            break;
        case SpellType.Heal:
            // Implement healing later
            dialogueText.text = $"You heal for {spellStrength} health!";
            break;
    }



    bool isDead = enemy.TakeDamage(spellStrength);
    
    if (isDead)
    {
        //end battle
    }
    else
    {
        EndPlayerTurn();
    }

}

}
