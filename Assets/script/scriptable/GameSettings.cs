using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable/GameSettings", order = 1)]

public class GameSettings : ScriptableObject
{
    public int startTryCount;
    public int minimumWordLength;
    public bool randomLength; // если включено, то слово для угадывания будет выбираться рандомом из "всех слов"
    // иначе выбирается по минимальной длине и после угадываний длина слов увеличивается
    
    public string currentSecretWord; 
    
    public List<List<string>> sortingLengths = new List<List<string>>(); // лист в листе для хранения слов по колву букв

    [HideInInspector] public List<string> uniqueWords;
    [HideInInspector]public string[] uniqueLetters;

    [SerializeField] public TextAsset[]  storyFiles;
    
    [HideInInspector] public int numberStoryFile;
    
    [HideInInspector] public int currentTryCount;
    [HideInInspector] public int currentScoreCount;
    [HideInInspector] public int currentWordNumber;
    [HideInInspector] public int currentWordLength;
    
    [HideInInspector] public Text[] wordTexts;
}
