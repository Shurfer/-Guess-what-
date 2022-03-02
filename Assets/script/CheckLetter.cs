using UnityEngine;
using UnityEngine.UI;

public class CheckLetter : MonoBehaviour
{
    private int closedLetters;

    private bool letterCatch;

    private GameSettings gameSettings;

    private void Start()
    {
        gameSettings = StaticScript.GameSettings;
        EventManager.FailGame.AddListener(FailGame);
    }

    void FailGame()
    {
        for (int j = 0; j < gameSettings.currentSecretWord.Length; j++)
        {
            if (!gameSettings.wordTexts[j].enabled)
                gameSettings.wordTexts[j].enabled = true;
        }
    }

    public bool CheckThisLetter(string letter) // вызывается из скрипта кнопки-буквы
    {
        if (StaticScript.play)
        {
            letterCatch = false;
            closedLetters = 0;
            for (int j = 0; j < gameSettings.currentSecretWord.Length; j++)
            {
                if (gameSettings.currentSecretWord[j] == letter[0])
                {
                    gameSettings.wordTexts[j].enabled = true;
                    gameSettings.wordTexts[j].GetComponentInParent<Image>().color = new Color32(255, 255, 255, 255);
                    letterCatch = true;
                }

                if (!gameSettings.wordTexts[j].enabled)
                    closedLetters++;
            }

            if (closedLetters == 0) // игрок угадал слово
                EventManager.SendWordGuessed();

            if (!letterCatch) // если игрок совершает ошибку
            {
                EventManager.SendFailChoose();
                if (gameSettings.currentTryCount < 1)
                {
                    gameSettings.currentTryCount = 0;
                    EventManager.SendFailGame();
                }
            }
        }

        return letterCatch;
    }
}