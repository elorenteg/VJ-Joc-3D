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

    private bool ghostBlueVisible;
    private bool ghostOrangeVisible;
    private bool ghostPinkVisible;
    private bool ghostRedVisible;
    private bool coinsVisible;
    private bool bonusVisible;

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

        ghostBlueVisible = true;
        ghostOrangeVisible = true;
        ghostPinkVisible = true;
        ghostRedVisible = true;
        coinsVisible = true;
        bonusVisible = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentLevel != 1)
            {
                currentLevel = 1;
                loadLevel(1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (currentLevel != 2)
            {
                currentLevel = 2;
                loadLevel(2);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (currentLevel != 3)
            {
                currentLevel = 3;
                loadLevel(3);
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ghostBlueVisible = !ghostBlueVisible;

            GameObject gameObjectGhost = GameObject.FindGameObjectWithTag(LevelCreator.TAG_GHOST_BLUE);
            gameObjectGhost.SetActive(ghostBlueVisible);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            ghostOrangeVisible = !ghostOrangeVisible;

            GameObject gameObjectGhost = GameObject.FindGameObjectWithTag(LevelCreator.TAG_GHOST_ORANGE);
            gameObjectGhost.SetActive(ghostOrangeVisible);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ghostPinkVisible = !ghostPinkVisible;

            GameObject gameObjectGhost = GameObject.FindGameObjectWithTag(LevelCreator.TAG_GHOST_PINK);
            gameObjectGhost.SetActive(ghostPinkVisible);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ghostRedVisible = !ghostRedVisible;

            GameObject gameObjectGhost = GameObject.FindGameObjectWithTag(LevelCreator.TAG_GHOST_RED);
            gameObjectGhost.SetActive(ghostRedVisible);
        }
        updateIA();
    }

    private void updateIA()
    {
        if (ghostBlueVisible) ghostBlueMove.onMove();
        if (ghostOrangeVisible) ghostOrangeMove.onMove(levelCreator.GetMap());
        if (ghostPinkVisible) ghostPinkMove.onMove();
        if (ghostRedVisible) ghostRedMove.onMove();
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

    public void bonusEaten()
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
}
