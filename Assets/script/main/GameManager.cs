using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameSettings gameSettings;

    [SerializeField] Text tryCountText;
    [SerializeField] Text tryScoreText;

    [SerializeField] TextAnalizator textAnalizator;
    [SerializeField] SecretWordCreate secretWordCreate;
    [SerializeField] RandomLettersCreate randomLettersCreated;

    [SerializeField] GameObject endFailMenu;
    [SerializeField] Text endText;

    [SerializeField] Material effectMaterial;
    [SerializeField] ParticleSystem[] particles;

    private string[] currentMasseges;

    private int txtFileNumber;

    private void Awake()
    {
        StaticScript.GameSettings = gameSettings;

        EventManager.SecretWordGuessed.AddListener(CurrentWordGuessed);

        EventManager.FailChoose.AddListener(FailChoose);
        EventManager.FailGame.AddListener(FailGame);

        currentMasseges = new string[4];
        currentMasseges[0] = "0";
        currentMasseges[1] = "Try guess word";
        currentMasseges[2] = "End Game".ToUpper();
        currentMasseges[3] = "WIN it, amazing!" + " " + "All words guessed!";
    }

    void Start()
    {
        NewGame();
    }
    public void NewGame()
    {
        ResetCurrentStart();

        textAnalizator.АnalysisText(gameSettings.storyFiles[gameSettings.numberStoryFile]);
        secretWordCreate.DisplayWordOnScreen();
        randomLettersCreated.DisplayLetterOnScreen();
    }

    void ResetCurrentStart() // сброс к стартовым 
    {
        StaticScript.endGame = false;
        StaticScript.play = true;

        gameSettings.currentWordNumber = 0;
        gameSettings.currentScoreCount = 0;
        gameSettings.currentWordNumber = 0;
        gameSettings.currentWordLength = gameSettings.minimumWordLength;
        tryScoreText.text = gameSettings.currentScoreCount.ToString();

        gameSettings.currentTryCount = gameSettings.startTryCount;
        tryCountText.text = gameSettings.currentTryCount.ToString();

        endText.text = currentMasseges[1];
        endText.color = new Color32(0, 200, 0, 255);

        endFailMenu.SetActive(false);
    }

    void FailChoose()
    {
        gameSettings.currentTryCount--;
        tryCountText.text = gameSettings.currentTryCount.ToString();
        particles[1].Play();
       // effectMaterial.mainTextureOffset = new Vector2(0.25f, 0.75f);
    }

    void FailGame() // закончились попытки - конец игры
    {
        StartCoroutine(timerToEndGAme());
        endText.text = currentMasseges[2];
        endText.color = new Color32(200, 0, 0, 255);
    }

    IEnumerator timerToEndGAme()
    {
        yield return new WaitForSeconds(1);
        EndGame();
    }

    void WinGame() //  все слова отгаданы - победа
    {
        EndGame();
        EventManager.SendWinGame();
        endText.text = currentMasseges[3];
        endText.color = new Color32(0, 200, 0, 255);
    }


    void EndGame()
    {
        StaticScript.play = false;
        StaticScript.endGame = true;
        endFailMenu.SetActive(true);
        SetPlayerPrefs();
    }

    void SetPlayerPrefs()
    {
        int bestScore = PlayerPrefs.GetInt("bestScore", 0);
        if (gameSettings.currentScoreCount > bestScore)
            PlayerPrefs.SetInt("bestScore", gameSettings.currentScoreCount);
    }

    void CurrentWordGuessed() // секретное слово отгадано
    {
        gameSettings.currentScoreCount += gameSettings.currentTryCount;
        tryScoreText.text = gameSettings.currentScoreCount.ToString();

        gameSettings.currentTryCount = gameSettings.startTryCount;
        tryCountText.text = gameSettings.currentTryCount.ToString();
        particles[0].Play();
       // effectMaterial.mainTextureOffset = new Vector2(0f, 0.75f);

        gameSettings.currentWordNumber++;
        if (gameSettings.currentSecretWord.Length == gameSettings.currentWordNumber)
        {
            gameSettings.currentWordNumber = 0;
            gameSettings.currentWordLength++;
            if (gameSettings.currentWordLength == gameSettings.sortingLengths.Count)
                WinGame();
        }

        if (gameSettings.currentWordNumber == gameSettings.uniqueWords.Count)
            WinGame();
        else
            StartCoroutine(timerToGetNewWord());
    }

    IEnumerator timerToGetNewWord()
    {
        yield return new WaitForSeconds(1f);
        secretWordCreate.DisplayWordOnScreen();
        randomLettersCreated.DisplayLetterOnScreen();
    }
}