using UnityEngine;
using System.Collections;


public class LevelManager : MonoBehaviour
{
    private static string texturesPath = "Textures\\";

    private static int INITIAL_LIFES = 3;
    private static string SCORE_LABEL = "SCORE";
    private static string LIFES_LABEL = "LIFES";
    private int currentScore;
    private int currentLifes;

    private GUIStyle normalFont;

    private Texture2D backgroundScoreTexture;
    private Texture2D backgroundLifesTexture;

    void Start()
    {
        initializeScore();
        initializeLifes();

        normalFont = new GUIStyle();
        normalFont.font = (Font)Resources.Load("Fonts/lilliput steps", typeof(Font));
        normalFont.fontSize = 28;
        normalFont.alignment = TextAnchor.UpperRight;
        normalFont.normal.textColor = Color.green;

        backgroundScoreTexture = Resources.Load(texturesPath + "score_tube") as Texture2D;
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
        // Score bg
        GUI.DrawTexture(new Rect(5, 5, 130, 50), backgroundScoreTexture);

        // Score label
        GUI.Label(new Rect(-80, 50, 200, 20), SCORE_LABEL, normalFont);

        // Score value
        GUI.Label(new Rect(-95, 11, 200, 20), currentScore.ToString(), normalFont);

        // Life bg
        GUI.DrawTexture(new Rect(Screen.width - 135, 5, 130, 50), backgroundLifesTexture);

        // Life label
        GUI.Label(new Rect(Screen.width - 220, 50, 200, 20), LIFES_LABEL, normalFont);

        // Life value
        GUI.Label(new Rect(Screen.width - 255, 11, 200, 20), currentLifes.ToString(), normalFont);
    }

    public void increaseScore(int quantity)
    {
        currentScore += quantity;
    }

    public int getScore()
    {
        return currentScore;
    }

    public void initializeScore()
    {
        currentScore = 0;
    }

    public void increaseLifes(int quantity)
    {
        currentLifes += quantity;
    }

    public int getLifes()
    {
        return currentLifes;
    }

    public void initializeLifes()
    {
        currentLifes = INITIAL_LIFES;
    }
}
