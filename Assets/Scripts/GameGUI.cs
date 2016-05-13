using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour
{

    private static string texturesPath = "Textures\\";

    private static string LEVEL_LABEL = "LEVEL";
    private static string SCORE_LABEL = "SCORE";
    private static string MAX_SCORE_LABEL = "HIGHSCORE";
    private static string LIFES_LABEL = "LIFES";

    private GUIStyle normalFont;
    private Texture2D backgroundLevelTexture;
    private Texture2D backgroundScoreTexture;
    private Texture2D backgroundLifesTexture;

    private LevelManager levelManager;

    void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        levelManager = gameManager.GetComponent<LevelManager>();

        normalFont = new GUIStyle();
        normalFont.font = (Font)Resources.Load("Fonts/lilliput steps", typeof(Font));
        normalFont.fontSize = 28;
        normalFont.alignment = TextAnchor.UpperRight;
        normalFont.normal.textColor = Color.green;

        backgroundScoreTexture = Resources.Load(texturesPath + "score_tube") as Texture2D;
        backgroundLevelTexture = backgroundScoreTexture;
        backgroundLifesTexture = backgroundScoreTexture;
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
        int currentScore = levelManager.getScore();
        int currentHighScore = levelManager.getHighScore();
        int currentLifes = levelManager.getLifes();
        int currentLevel = levelManager.getLevel();

        // Score bg
        GUI.DrawTexture(new Rect(5, 5, 135, 50), backgroundScoreTexture);

        // Score label
        GUI.Label(new Rect(-80, 50, 200, 20), SCORE_LABEL, normalFont);

        // Score value
        GUI.Label(new Rect(-95, 11, 200, 20), currentScore.ToString(), normalFont);

        // Max. Score bg
        GUI.DrawTexture(new Rect(Screen.width - 185, 5, 180, 50), backgroundScoreTexture);

        // Max. Score label
        GUI.Label(new Rect(Screen.width - 205, 50, 200, 20), MAX_SCORE_LABEL, normalFont);

        // Max. Score value
        GUI.Label(new Rect(Screen.width - 235, 11, 200, 20), currentHighScore.ToString(), normalFont);

        // Life bg
        GUI.DrawTexture(new Rect(5, Screen.height - 55, 135, 50), backgroundLifesTexture);

        // Life label
        GUI.Label(new Rect(-80, Screen.height - 90, 200, 20), LIFES_LABEL, normalFont);

        // Life value
        GUI.Label(new Rect(-95, Screen.height - 50, 200, 20), currentLifes.ToString(), normalFont);

        // Level bg
        GUI.DrawTexture(new Rect(Screen.width - 140, Screen.height - 55, 135, 50), backgroundLevelTexture);

        // Level label
        GUI.Label(new Rect(Screen.width - 215, Screen.height - 90, 200, 20), LEVEL_LABEL, normalFont);

        // Level value
        GUI.Label(new Rect(Screen.width - 235, Screen.height - 50, 200, 20), currentLevel.ToString(), normalFont);
    }
}
