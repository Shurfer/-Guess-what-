using UnityEngine;
using UnityEngine.UI;

public class Leadeboard : MonoBehaviour
{
    [SerializeField] private RectTransform prefab;
    [SerializeField] private RectTransform content;

    [SerializeField] private Text endScoreText;

    [SerializeField] GameObject leaderboardMenu;
    [SerializeField] Text placeHolder;
    [SerializeField] Scrollbar scrollbar;

    private PrefabModel[] resultsPrefabModel;

    private string[] leadersName;
    private int[] leaderScore;

    private string playerName;
    private int bestScore;

    Text nameText;

    private void Start()
    {
        leadersName = new[] {"Alex", "Антуа", "Зил и боба", "Четвертый", "Илон", "Каша"};
    }

    public void LeaderboardActivate() // вызывается из кнопки "Experts"
    {
        leaderboardMenu.SetActive(true);

        playerName = PlayerPrefs.GetString("playerName", "...");
        placeHolder.text = playerName;
        bestScore = PlayerPrefs.GetInt("bestScore", 0);
        leaderScore = new[] {75, 50, 33, 15, 6, bestScore};

        for (int i = 0; i < leaderScore.Length; i++)
        {
            for (int j = 0; j < leaderScore.Length - 1; j++)
            {
                if (leaderScore[j] < leaderScore[j + 1])
                {
                    int z = leaderScore[j];
                    leaderScore[j] = leaderScore[j + 1];
                    leaderScore[j + 1] = z;
                }
            }
        }

        CreatePrefabModels();
        UpdateContent();
        // Time.timeScale = 0;
    }

    void CreatePrefabModels()
    {
        resultsPrefabModel = new PrefabModel[leaderScore.Length];
        for (int i = 0; i < leaderScore.Length; i++)
        {
            resultsPrefabModel[i] = new PrefabModel();
            resultsPrefabModel[i].number = (i + 1).ToString();
            resultsPrefabModel[i].name = leadersName[i];
            if (leaderScore[i] == bestScore)
                resultsPrefabModel[i].name = playerName;
            resultsPrefabModel[i].score = leaderScore[i].ToString();
        }

        endScoreText.text = bestScore.ToString();
    }

    void UpdateContent()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var model in resultsPrefabModel)
        {
            var instance = Instantiate(prefab.gameObject, content, false);
            PrefabView view = new PrefabView(instance.transform);
            view.numberText.text = model.number;
            if (model.name == playerName)
            {
                nameText = view.nameText;
            }

            view.nameText.text = model.name;
            view.scoreText.text = model.score;
        }

        scrollbar.value = 0;
    }

    public class PrefabModel
    {
        public string number;
        public string name;
        public string score;
    }

    public class PrefabView
    {
        public Text numberText;
        public Text nameText;
        public Text scoreText;

        public PrefabView(Transform rootView)
        {
            var texts = rootView.GetComponentsInChildren<Text>();
            numberText = texts[0];
            nameText = texts[1];
            scoreText = texts[2];
        }
    }

    public void ChangeName(string name)
    {
        PlayerPrefs.SetString("playerName", name);
        nameText.text = name;
    }

    /*  public void BackToPlay()
      {
          leaderboardMenu.SetActive(false);
          Time.timeScale = 1;
      }*/
}