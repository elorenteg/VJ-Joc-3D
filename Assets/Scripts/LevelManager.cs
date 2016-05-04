using UnityEngine;
using System.Collections;


public class LevelManager : MonoBehaviour
{
    private static string texturesPath = "Textures\\";

    private int currentScore;
    private int currentLifes;

    private GUIStyle normalFont;

    private Texture2D backgroundScoreTexture;

    void Start()
    {
        initializeScore();
        initializeLifes();

        normalFont = new GUIStyle();
        normalFont.fontSize = 28;
        normalFont.alignment = TextAnchor.UpperRight;

        backgroundScoreTexture = Resources.Load(texturesPath + "score_tube_preview") as Texture2D;
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
        GUI.DrawTexture(new Rect(0, 0, 110, 50), backgroundScoreTexture);

        // Score text
        GUI.Label(new Rect(-100, 10, 200, 20), "" + currentScore, normalFont);
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
        currentLifes = 0;
    }
}
