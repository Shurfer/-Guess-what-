using UnityEngine.Events;

public class EventManager
{
    public static UnityEvent SecretWordGuessed = new UnityEvent();
    public static UnityEvent WinGame = new UnityEvent();

    public static UnityEvent FailChoose = new UnityEvent();
    public static UnityEvent FailGame = new UnityEvent();

    public static void SendWordGuessed()
    {
        SecretWordGuessed.Invoke();
    }

    public static void SendWinGame()
    {
        WinGame.Invoke();
    }

    public static void SendFailChoose()
    {
        FailChoose.Invoke();
    }

    public static void SendFailGame()
    {
        FailGame.Invoke();
    }
}