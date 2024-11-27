using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WordDisplay : MonoBehaviour
{
    public TextMeshProUGUI text;
    WordManager wordManager;
    WordSpawner wordSpawner;

    public float shrinkSpeed = .25f; //chang shrink speed with difficulty 
    public float minScale = .25f;


    private void Start() 
    {
        wordManager = FindObjectOfType<WordManager>();
        wordSpawner = FindObjectOfType<WordSpawner>();
                
        // Start the shrinking process
        StartCoroutine(ShrinkCoroutine());
        
    }
    public void SetWord (string word)
    {
        text.text = word;
    }
    public void RemoveLetter()
    {
        text.text = text.text.Remove(0, 1);
        text.color = Color.red;
    }
    public void RemoveWord()
    {           
        //remove from list
        //destroy
        if (gameObject != null)
            Destroy(gameObject);
    }

    public void WordFailed()
    {
        //add next word (null check syntax)
        wordManager?.WordTimedOut();

        if (wordSpawner != null)
            wordSpawner.spellTimer -= 5f;

        //remove from list

        //destroy
        Destroy(gameObject);
    }
    private void Update() 
    {
        //if want to fall down the screen
        //transform.Translate(0f, -moveSpeed * Time.deltaTime, 0f);

        //shrink


        if (wordSpawner.currentSpellState == SpellState.Inactive)
        {
            RemoveWord();
        }

    }



    private IEnumerator ShrinkCoroutine()
    {
        // Shrink the object over time
        while (transform.localScale.x > minScale)
        {
            // Shrink the object uniformly (scaling down)
            transform.localScale -= new Vector3(shrinkSpeed, shrinkSpeed, shrinkSpeed) * Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Once the object has shrunk to the threshold, remove and destroy it
        WordFailed();
    }
}
