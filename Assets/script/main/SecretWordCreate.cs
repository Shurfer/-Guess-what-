using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecretWordCreate : MonoBehaviour
{
    [SerializeField] Transform contentWordTr;
    [SerializeField] GameObject letterForWordPrefab;

    [SerializeField] GridLayoutGroup gridContent;

    private List<string> tempList;

    private Vector2 cellSize;

    private int massiveLength;

    private GameSettings gameSettings;

    private void Start()
    {
        gameSettings = StaticScript.GameSettings;
    }
    
    public void DisplayWordOnScreen() // создает блоки с буквами по длине подходящего слова 
    {
        foreach (Transform child in contentWordTr)
        {
            Destroy(child.gameObject);
        }

        if (gameSettings.randomLength) 
        {
            massiveLength = gameSettings.uniqueWords.Count;
            tempList = gameSettings.uniqueWords;
        }
        else
        {
            massiveLength = gameSettings.sortingLengths[gameSettings.currentWordLength].Count;
            tempList = gameSettings.sortingLengths[gameSettings.currentWordLength];
        }

        int wordNumber = gameSettings.currentWordNumber;

        if (wordNumber < massiveLength)
        {
            gameSettings.currentSecretWord = tempList[wordNumber]; // запись нового секретного слова

            gameSettings.wordTexts = new Text[tempList[wordNumber].Length];
            for (int j = 0; j < tempList[wordNumber].Length; j++)
            {
                GameObject letter = Instantiate(letterForWordPrefab, contentWordTr, false);
                letter.GetComponent<Image>().color = new Color32(255, 255, 255, 100);
                gameSettings.wordTexts[j] = letter.GetComponentInChildren<Text>();
                gameSettings.wordTexts[j].text = tempList[wordNumber][j].ToString();
                gameSettings.wordTexts[j].enabled = false;
            }

            if (gameSettings.currentSecretWord.Length > 10)
                cellSize = new Vector2(70, 70);
            else
                cellSize = new Vector2(100, 100);

            gridContent.cellSize = cellSize;
        }
    }
}