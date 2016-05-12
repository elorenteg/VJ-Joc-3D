using UnityEngine;
using System.Collections;


public class LevelManager : MonoBehaviour
{
    private static string texturesPath = "Textures\\";

    private static int TOTAL_LEVEL = 2;
    private static int INITIAL_LEVEL = 1;

    private static int INITIAL_LIFES = 3;
    private static string LEVEL_LABEL = "LEVEL";
    private static string SCORE_LABEL = "SCORE";
    private static string LIFES_LABEL = "LIFES";
    private int currentLevel;
    private int currentScore;
    private int currentLifes;
    private int remainingCoins;

    private static int[] COINS_NUMBER = { 1, 4 };

    private static int COIN_SCORE = 1;

    private GUIStyle normalFont;
    private Texture2D backgroundScoreTexture;
    private Texture2D backgroundLifesTexture;

    private LevelCreator levelCreator;

    void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        levelCreator = gameManager.GetComponent<LevelCreator>();

        normalFont = new GUIStyle();
        normalFont.font = (Font)Resources.Load("Fonts/lilliput steps", typeof(Font));
        normalFont.fontSize = 28;
        normalFont.alignment = TextAnchor.UpperRight;
        normalFont.normal.textColor = Color.green;

        backgroundScoreTexture = Resources.Load(texturesPath + "score_tube") as Texture2D;
        backgroundLifesTexture = backgroundScoreTexture;

        startGame();
    }

    void startGame()
    {
        currentLevel = INITIAL_LEVEL;
        initializeScore();
        initializeLifes();

        loadLevel(currentLevel);
    }

    void loadLevel(int level)
    {
        levelCreator.loadLevel(level);
        remainingCoins = COINS_NUMBER[level - 1];
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

        // Level bg
        GUI.DrawTexture(new Rect(0, Screen.height - 55, 130, 50), backgroundLifesTexture);

        // Level label
        GUI.Label(new Rect(-80, Screen.height - 90, 200, 20), LEVEL_LABEL, normalFont);

        // Level value
        GUI.Label(new Rect(-95, Screen.height - 50, 200, 20), currentLevel.ToString(), normalFont);
    }

    public void coinEaten()
    {
        // TODO Se deja asi por si se quiere implementar un multiplicador
        int multiplier = 1;
        increaseScore(COIN_SCORE * multiplier);
        --remainingCoins;
        Debug.Log("Remaining coins: " + remainingCoins);

        if (remainingCoins == 0)
        {
            // Fin del juego
            if (currentLevel == TOTAL_LEVEL)
            {

            }
            else
            {
                //TODO Imagen final de nivel
                ++currentLevel;
                loadLevel(currentLevel);
            }
        }
    }

    private void increaseScore(int quantity)
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

    public void increaseLife()
    {
        ++currentLifes;
    }

    public void decreaseLifes()
    {
        --currentLifes;

        if (currentLifes == -1)
        {
            // TODO Mostrar mensaje fin del juego

            // Y luego
            startGame();
        }
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
