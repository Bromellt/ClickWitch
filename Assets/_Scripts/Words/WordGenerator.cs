using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;

public class WordGenerator : MonoBehaviour
{
    private static string [] wordList = { "abra", "kadabra", "alakazam", "fish", "tomato", "pazzaz"};

    private static string [] fireWordList = { "blaze", "hot", "fuele", "beef", "ignise", "combust"};



    public static string GetRandomWord()
    {
        int randomIndex = Random.Range(0, wordList.Length);
        string randomWord = wordList[randomIndex];
        return randomWord;
    }
    

    public static string GetRandomFireWord()
    {
        int randomIndex = Random.Range(0, fireWordList.Length);
        string randomWord = wordList[randomIndex];
        return randomWord;
    }

}
