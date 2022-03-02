using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RandomLettersCreate : MonoBehaviour
{
    [SerializeField] Transform contentLetterTr;
    [SerializeField] GameObject letterPrefab;

    [SerializeField] GridLayoutGroup gridLayout;
    [SerializeField] CheckLetter checkLetter;

    private int newLetterValue;

    private string randomLetter;
    private string[] leterMassiveToChoose; //массив букв, из которых выбирают
    private string[] freeLettersMassive;

    private List<string> listOfLetters;

    // из словаря этих букв выбирается рандомная
    Dictionary<string, string> freeLetters = new Dictionary<string, string>();

    private GameSettings gameSettings;

    private string alphabet = "abcdefghijklmnopqrstuvwxyz";

    private void Start()
    {
        gameSettings = StaticScript.GameSettings;
        EventManager.FailGame.AddListener(ContentClear);
        EventManager.WinGame.AddListener(ContentClear);
    }

    void ContentClear()
    {
        foreach (Transform child in contentLetterTr)
        {
            Destroy(child.gameObject);
        }
    }

    public void DisplayLetterOnScreen() // выводит доступные буквы для угадывания на экран 
    {
        ContentClear();

        gridLayout.enabled = true;

        listOfLetters = new List<string>();

        for (int j = 0; j < gameSettings.currentSecretWord.Length; j++)
            listOfLetters.Add(gameSettings.currentSecretWord[j].ToString());

        var uniqueLetters = listOfLetters.Distinct().ToList();

        gameSettings.uniqueLetters = new string[uniqueLetters.Count];
        for (int j = 0; j < uniqueLetters.Count; j++)
        {
            gameSettings.uniqueLetters[j] = uniqueLetters[j];
        }

        DictionaryCase();
        NewRandowLetterJoin();
        CreateRandomButtons();
    }

    void DictionaryCase() // словарь хранит "оставшиеся буквы"
    {
        freeLetters.Clear();

        for (int j = 0; j < alphabet.Length; j++)
            freeLetters.Add(alphabet[j].ToString(), alphabet[j].ToString()); // добавляется весь алфавит

        int value;
        if (gameSettings.currentSecretWord.Length % 2 == 0)
            value = 0;
        else
            value = 1;

        leterMassiveToChoose =
            new string [gameSettings.startTryCount + 1 + gameSettings.currentSecretWord.Length + value];

        for (int j = 0; j < gameSettings.uniqueLetters.Length; j++)
        {
            freeLetters.Remove(gameSettings.uniqueLetters[j]); // удаляются буквы секретного слова 
            leterMassiveToChoose[j] = gameSettings.uniqueLetters[j];
        }

        newLetterValue = gameSettings.uniqueLetters.Length; // номер новой буквы в массиве
    }

    void NewRandowLetterJoin() // добавление новых рандомных букв 
    {
        int i = 0;
        freeLettersMassive = new String[freeLetters.Count];

        foreach (var letter in freeLetters)
        {
            freeLettersMassive[i] = letter.Value;
            i++;
        }

        randomLetter = freeLettersMassive[Random.Range(0, freeLettersMassive.Length)];

        freeLetters.Remove(randomLetter);

        leterMassiveToChoose[newLetterValue] = randomLetter; // запись новой буквы в массив

        newLetterValue++;
        if (newLetterValue < leterMassiveToChoose.Length)
            NewRandowLetterJoin();
    }

    void CreateRandomButtons() // создание кнопок с буквами
    {
        for (int i = 0; i < leterMassiveToChoose.Length; i++) // смена порядка букв
        {
            string temp = leterMassiveToChoose[i];
            int r = Random.Range(0, leterMassiveToChoose.Length);
            leterMassiveToChoose[i] = leterMassiveToChoose[r];
            leterMassiveToChoose[r] = temp;
        }

        for (int j = 0; j < leterMassiveToChoose.Length; j++) // кнопки
        {
            GameObject letter = Instantiate(letterPrefab, contentLetterTr, false);
            letter.GetComponentInChildren<Text>().text = leterMassiveToChoose[j];
            letter.GetComponent<ButtonClick>().checkLetter = checkLetter;
        }

        StartCoroutine(gridLayoutOff());
    }

    IEnumerator gridLayoutOff() // чтоб кнопки перестали сдвигаться при уничтожении
    {
        yield return new WaitForSeconds(0.5f);
        gridLayout.enabled = false;
    }
}