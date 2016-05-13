using UnityEngine;
using System.Collections;


public class LevelManager : MonoBehaviour
{

    private static int TOTAL_LEVEL = 2;
    private static int INITIAL_LEVEL = 1;

    private static int INITIAL_LIFES = 3;
    private int currentLevel;
    private int currentScore;
    private int currentHighScore;
    private int currentLifes;
    private int remainingCoins;

    private static int[] COINS_NUMBER = { 1, 4 };

    private static int COIN_SCORE = 1;

    private LevelCreator levelCreator;
    private DataManager dataManager;

    private GhostBlueMove ghostBlueMove;
    private GhostOrangeMove ghostOrangeMove;
    private GhostPinkMove ghostPinkMove;
    private GhostRedMove ghostRedMove;

    void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        levelCreator = gameManager.GetComponent<LevelCreator>();
        dataManager = gameManager.GetComponent<DataManager>();
       
        currentHighScore = dataManager.readMaxScore();

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

        GameObject gameObjectGhost = GameObject.FindGameObjectWithTag(LevelCreator.TAG_GHOST_BLUE);
        ghostBlueMove = gameObjectGhost.GetComponent<GhostBlueMove>();

        gameObjectGhost = GameObject.FindGameObjectWithTag(LevelCreator.TAG_GHOST_ORANGE);
        ghostOrangeMove = gameObjectGhost.GetComponent<GhostOrangeMove>();

        gameObjectGhost = GameObject.FindGameObjectWithTag(LevelCreator.TAG_GHOST_PINK);
        ghostPinkMove = gameObjectGhost.GetComponent<GhostPinkMove>();

        gameObjectGhost = GameObject.FindGameObjectWithTag(LevelCreator.TAG_GHOST_RED);
        ghostRedMove = gameObjectGhost.GetComponent<GhostRedMove>();

        remainingCoins = COINS_NUMBER[level - 1];
    }

    void Update()
    {
        updateIA();
    }

    private void updateIA()
    {
        ghostBlueMove.onMove();
        ghostOrangeMove.onMove();
        ghostPinkMove.onMove();
        ghostRedMove.onMove();
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

    public void initializeScore()
    {
        currentScore = 0;
    }

    public int getScore()
    {
        return currentScore;
    }

    private void increaseScore(int quantity)
    {
        currentScore += quantity;
        if (currentScore > currentHighScore)
        {
            currentHighScore = currentScore;
            dataManager.saveMaxScore(currentScore);
        }
    }

    public void initializeLifes()
    {
        currentLifes = INITIAL_LIFES;
    }

    public int getLifes()
    {
        return currentLifes;
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

    public int getLevel()
    {
        return currentLevel;
    }

    public int getHighScore()
    {
        return currentHighScore;
    }
}
