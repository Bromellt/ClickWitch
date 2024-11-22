using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordManager : MonoBehaviour
{
    public List<Word> words;
    bool hasActiveWord;
    Word activeWord;
    public WordSpawner wordSpawner;
    public GameObject spellStrengthCircle;
    float sizeChange = .2f;

    //track the current spell

    public int wordsTyped;


    public void AddWord()
    {
        if (wordSpawner.currentSpellState == SpellState.Active)
        {
            Word word = new Word(WordGenerator.GetRandomWord(), wordSpawner.SpawnWord());
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
            UpdateSpellStrengthIndicator(sizeChange*2);
            AddWord();
        }
        
    }
    public void ResetWordCount()
    {
        wordsTyped = 0; // Reset counter after the spell ends
        ResetSpellIndicator(); // Reset circle size
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
        UpdateSpellStrengthIndicator(-sizeChange*2);
        AddWord(); // Add a new word
    }

    void UpdateSpellStrengthIndicator(float scale)
    {
        if (spellStrengthCircle != null)
        {
            // Update the circle's scale in real time
            spellStrengthCircle.transform.localScale += new Vector3(scale, scale, scale);
        }
    }
    void ResetSpellIndicator()
    {
        spellStrengthCircle.transform.localScale = new Vector3(1, 1, 1);
    }

    public void WrongKeyPressed()
    {
        //time penalty
        wordSpawner.spellTimer -= .5f;
        UpdateSpellStrengthIndicator(-sizeChange);
        //flash screen red
        //reduce spell power
        
    }


}
