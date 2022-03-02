
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TextAnalizator : MonoBehaviour
{
    private List<string> listOfWords = new List<string>();

    private string cleanWord;

    private int uniqueWordsCount;
    private int random;
    private int i;

    GameSettings gameSettings;

    private char[] charsToTrim =
    {
        ' ', '\n', '\'', '`', '"', '_', '(', ')', '[', ']', '*', '+', '-', '=', '/', '\\', '|',
        '?', '!', ',', '.', ';', ':', '<', '>', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
    };

    private void Start()
    {
        gameSettings = StaticScript.GameSettings;
    }

    public void АnalysisText(TextAsset txtFile) // разделяет текст на слова, очищает от знаков, оставляет уникальные
    {
        string[] AllWords = txtFile.text.Split(charsToTrim); // слова разделяются

        for (int j = 0; j < AllWords.Length; j++)
        {
            cleanWord = AllWords[j].Trim(charsToTrim).ToLower(); // очищение слова от знаков и больших букв
            if (cleanWord != null && cleanWord.Length > 2)
            {
                if (cleanWord[cleanWord.Length - 1].ToString() == "s") // множестенное чилсо
                {
                    string singleWord = cleanWord.Substring(0, cleanWord.Length - 1); // удаление "лишних" букв
                    cleanWord = singleWord;
                    // здесь должна быть проверка на остальные написания множественного числа, но оставим это на апдейт
                    // проверка на существительное туда же
                }

                if (cleanWord.Length >= gameSettings.minimumWordLength)
                    listOfWords.Add(cleanWord);
            }
        }

        gameSettings.uniqueWords = new List<string>(listOfWords.Count);
        gameSettings.uniqueWords = listOfWords.Distinct().ToList(); //удаление повторяющихся слов

        for (int k = 0; k <= 20; k++)
        {
            List<string> oneLengthWords = new List<string>();
            gameSettings.sortingLengths.Add(oneLengthWords);
        }

        for (int j = 0; j < gameSettings.uniqueWords.Count; j++)
        {
            string word = gameSettings.uniqueWords[j];
            gameSettings.sortingLengths[word.Length].Add(word);
        }


        for (int i = 0; i < gameSettings.uniqueWords.Count; i++) // смена порядка в массиве "всех уникальных"
        {
            string temp = gameSettings.uniqueWords[i];
            int random = Random.Range(0, gameSettings.uniqueWords.Count);
            gameSettings.uniqueWords[i] = gameSettings.uniqueWords[random];
            gameSettings.uniqueWords[random] = temp;
        }

        foreach (var list in gameSettings.sortingLengths) // рандом в списках по длине слова
        {
            if (list == null)
                list.Add("A");

            for (int j = 0; j < list.Count; j++)
            {
                string temp = list[j];
                random = Random.Range(0, list.Count);
                list[j] = list[random];
                list[random] = temp;
            }
        }
    }
}