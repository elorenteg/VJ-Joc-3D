using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour
{

    public static string TITLE_END_OF_LEVEL_TEXT = "Fi de nivell";
    public static string MESSAGE_END_OF_LEVEL_TEXT = "Nivell finalitzat\nPrem ENTER";

    private static string texturesPath = "Textures\\";

    private static string LEVEL_LABEL = "LEVEL";
    private static string SCORE_LABEL = "SCORE";
    private static string MAX_SCORE_LABEL = "HIGHSCORE";
    private static string LIFES_LABEL = "LIFES";

    private GUIStyle infoFont;
    private GUIStyle titleFont;
    private GUIStyle messageFont;
    private Texture2D backgroundLevelTexture;
    private Texture2D backgroundScoreTexture;
    private Texture2D backgroundLifesTexture;
    private Texture2D backgroundMessageTexture;

    private LevelManager levelManager;

    private bool isMessageInQueue;

    private struct Message
    {
        public string titleToShow;
        public string messageToShow;
        public int xTitle;
        public int yTitle;
        public int xMessage;
        public int yMessage;
        public int xBackground;
        public int yBackground;
        public int widthBackground;
        public int heightBackground;
    };

    private Message message1;

    void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        levelManager = gameManager.GetComponent<LevelManager>();

        infoFont = new GUIStyle();
        infoFont.font = (Font)Resources.Load("Fonts/lilliput steps", typeof(Font));
        infoFont.fontSize = 28;
        infoFont.alignment = TextAnchor.UpperRight;
        infoFont.normal.textColor = Color.green;

        titleFont = new GUIStyle();
        titleFont.font = (Font)Resources.Load("Fonts/lilliput steps", typeof(Font));
        titleFont.fontSize = 28;
        titleFont.alignment = TextAnchor.MiddleCenter;
        titleFont.normal.textColor = Color.black;

        messageFont = new GUIStyle();
        messageFont.font = (Font)Resources.Load("Fonts/lilliput steps", typeof(Font));
        messageFont.fontSize = 26;
        messageFont.alignment = TextAnchor.MiddleCenter;
        messageFont.normal.textColor = Color.cyan;

        backgroundScoreTexture = Resources.Load(texturesPath + "score_tube") as Texture2D;
        backgroundLevelTexture = backgroundScoreTexture;
        backgroundLifesTexture = backgroundScoreTexture;

        backgroundMessageTexture = Resources.Load(texturesPath + "message_paper") as Texture2D;

        isMessageInQueue = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Application.Quit();
        }
    }

    void OnGUI()
    {
        showGUIInformation();

        if (isMessageInQueue)
        {
            showMessageInQueue(message1);
        }
    }

    private void showGUIInformation()
    {
        int currentScore = levelManager.getScore();
        int currentHighScore = levelManager.getHighScore();
        int currentLifes = levelManager.getLifes();
        int currentLevel = levelManager.getLevel();

        // Score bg
        GUI.DrawTexture(new Rect(5, 5, 135, 50), backgroundScoreTexture);

        // Score label
        GUI.Label(new Rect(-80, 50, 200, 20), SCORE_LABEL, infoFont);

        // Score value
        GUI.Label(new Rect(-95, 11, 200, 20), currentScore.ToString(), infoFont);

        // Max. Score bg
        GUI.DrawTexture(new Rect(Screen.width - 185, 5, 180, 50), backgroundScoreTexture);

        // Max. Score label
        GUI.Label(new Rect(Screen.width - 205, 50, 200, 20), MAX_SCORE_LABEL, infoFont);

        // Max. Score value
        GUI.Label(new Rect(Screen.width - 235, 11, 200, 20), currentHighScore.ToString(), infoFont);

        // Life bg
        GUI.DrawTexture(new Rect(5, Screen.height - 55, 135, 50), backgroundLifesTexture);

        // Life label
        GUI.Label(new Rect(-80, Screen.height - 90, 200, 20), LIFES_LABEL, infoFont);

        // Life value
        GUI.Label(new Rect(-95, Screen.height - 50, 200, 20), currentLifes.ToString(), infoFont);

        // Level bg
        GUI.DrawTexture(new Rect(Screen.width - 140, Screen.height - 55, 135, 50), backgroundLevelTexture);

        // Level label
        GUI.Label(new Rect(Screen.width - 215, Screen.height - 90, 200, 20), LEVEL_LABEL, infoFont);

        // Level value
        GUI.Label(new Rect(Screen.width - 235, Screen.height - 50, 200, 20), currentLevel.ToString(), infoFont);
    }

    private void showMessageInQueue(Message message)
    {
        // Message bg
        GUI.DrawTexture(new Rect(message1.xBackground, message1.yBackground, message1.widthBackground, message1.heightBackground), backgroundMessageTexture);

        // Message title
        GUI.Label(new Rect(message1.xTitle, message1.yTitle, 0, 0), message.titleToShow, titleFont);

        // Message content
        GUI.Label(new Rect(message1.xMessage, message1.yMessage, 0, 0), message.messageToShow, messageFont);
    }

    public void addMessageToQueue(string title, string message)
    {
        message1.titleToShow = title;
        message1.messageToShow = message;

        if (title == TITLE_END_OF_LEVEL_TEXT)
        {
            message1.xTitle = (int)((Screen.width / 2) * 1.0);
            message1.yTitle = (int)((Screen.height / 2) * 0.75);
            message1.widthBackground = (int)((Screen.width / 2) * 0.9);
            message1.heightBackground = (int)((Screen.height / 2) * 1.1);
            message1.xBackground = Screen.width / 2 - message1.widthBackground / 2;
            message1.yBackground = Screen.height / 2 - message1.heightBackground / 2;
        }

        if (message == MESSAGE_END_OF_LEVEL_TEXT)
        {
            message1.xMessage = (int)((Screen.width / 2) * 1.0);
            message1.yMessage = (int)((Screen.height / 2) * 1.1);
        }

        isMessageInQueue = true;
    }

    public void removeMessage()
    {
        isMessageInQueue = false;
    }
}
