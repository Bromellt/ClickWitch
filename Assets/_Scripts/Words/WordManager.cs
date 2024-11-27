using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WordManager : MonoBehaviour
{
    public List<Word> words;
    bool hasActiveWord;
    Word activeWord;
    public WordSpawner wordSpawner;



    public IndicatorControl spellIndicator;
    float sizeChange = .1f;


    public int wordsTyped;
    public int mistakes;
    public int wordsMissed;
    Spell currentSpell;





    public void AddWord(Spell spell)
    {
        if (wordSpawner.currentSpellState == SpellState.Active)
        {
            currentSpell = spell;
            string randomWord = spell.GetRandomWord();
            Word word = new Word(randomWord, wordSpawner.SpawnWord());
            words.Add(word);
        }
        
    }


    public void TypeLetter(char letter)
    {
        if (hasActiveWord)
        {
            //check if letter was next
            //remove it from the word
            if (activeWord.GetNextLetter() == letter)
            {
                activeWord.TypeLetter();
            }
            else
            {
                WrongKeyPressed();
            }
        }
        else
        {
            foreach (Word word in words)
            {
                if (word.GetNextLetter() == letter)
                {
                    activeWord = word;
                    hasActiveWord = true;
                    word.TypeLetter();
                    break; //don't keep searching 
                }
            }
        }


        if(hasActiveWord && activeWord.WordTyped())
        {
            hasActiveWord = false;
            words.Remove(activeWord); 
            wordsTyped++;
            spellIndicator.UpdateSize(sizeChange*2);
            AddWord(currentSpell);
        }
        
    }
    public void ResetWordCount()
    {
        wordsTyped = 0; // Reset counter after the spell ends
        spellIndicator.ResetSize(); // Reset circle size
        
    }
    public int GetWordsTyped()
    {
        return wordsTyped; // Return the current count
    }
    
    public void WordTimedOut()
    {
        if(hasActiveWord)
        {
            words.Remove(activeWord);  // Remove the timed out word
        }
        else
        {
            words.Clear();
        }
        
        hasActiveWord = false;
        spellIndicator.UpdateSize(-sizeChange*2);
        wordsMissed++;
        AddWord(currentSpell); // Add a new word
    }


    public void WrongKeyPressed()
    {
        //time penalty
        //wordSpawner.spellTimer -= .5f;
        mistakes++;
        spellIndicator.UpdateSize(-sizeChange);
        //flash screen red
        //reduce spell power
        
    }

}
