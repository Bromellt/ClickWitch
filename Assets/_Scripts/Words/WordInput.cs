using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordInput : MonoBehaviour
{

    public WordManager wordManager;
    private void Update() 
    {
        foreach(char letter in Input.inputString)
        {
            wordManager.TypeLetter(letter);
        }
    }
}
