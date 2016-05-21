using UnityEngine;
using System.Collections;


public class LevelManager : MonoBehaviour
{

    private static int TOTAL_LEVEL = 2;
    private static int INITIAL_LEVEL = 1;
    private static int INITIAL_LIFES = 3;
    private static int[] COINS_NUMBER = { 30, 4 };
    private static int COIN_SCORE = 1;

    private int currentLevel;
    private int currentScore;
    private int currentHighScore;
    private int currentLifes;
    private int remainingCoins;

    private LevelCreator levelCreator;
    private DataManager dataManager;
    private GameGUI gameGUI;
    private PacmanMove pacmanMove;

    private bool ghostBlueVisible;
    private bool ghostOrangeVisible;
    private bool ghostPinkVisible;
    private bool ghostRedVisible;

    private GhostBlueMove ghostBlueMove;
    private GhostOrangeMove ghostOrangeMove;
    private GhostPinkMove ghostPinkMove;
    private GhostRedMove ghostRedMove;

    private bool gamePaused;

    private static int TIME_BONUS_PACMAN_KILLS_GHOST = 10; //10 seconds
    private bool bonusPacmanKillsGhost;
    private float bonusPacmanKillsGhostRemaining;

    void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        levelCreator = gameManager.GetComponent<LevelCreator>();
        dataManager = gameManager.GetComponent<DataManager>();
        gameGUI = gameManager.GetComponent<GameGUI>();

        currentHighScore = dataManager.readMaxScore();
        gamePaused = false;
        bonusPacmanKillsGhost = false;
        bonusPacmanKillsGhostRemaining = 0.0f;

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

        GameObject gameObjectGhost = GameObject.FindGameObjectWithTag(Globals.TAG_GHOST_BLUE);
        ghostBlueMove = gameObjectGhost.GetComponent<GhostBlueMove>();

        gameObjectGhost = GameObject.FindGameObjectWithTag(Globals.TAG_GHOST_ORANGE);
        ghostOrangeMove = gameObjectGhost.GetComponent<GhostOrangeMove>();

        gameObjectGhost = GameObject.FindGameObjectWithTag(Globals.TAG_GHOST_PINK);
        ghostPinkMove = gameObjectGhost.GetComponent<GhostPinkMove>();

        gameObjectGhost = GameObject.FindGameObjectWithTag(Globals.TAG_GHOST_RED);
        ghostRedMove = gameObjectGhost.GetComponent<GhostRedMove>();

        GameObject gameObjectPacman = GameObject.FindGameObjectWithTag(Globals.TAG_PACMAN);
        pacmanMove = gameObjectPacman.GetComponent<PacmanMove>();

        remainingCoins = COINS_NUMBER[level - 1];

        ghostBlueVisible = true;
        ghostOrangeVisible = true;
        ghostPinkVisible = true;
        ghostRedVisible = true;
    }

    void Update()
    {

        if (!gamePaused)
        {
            updateIA();
            updateTimeBonus();
        }

        if (gamePaused && Input.GetKey(KeyCode.Return))
        {
            if (remainingCoins == 0)
            {
                ++currentLevel;
                loadLevel(currentLevel);
            }

            endMessage();
        }

        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Application.Quit();
        }

        if (Globals.ARE_CHEATS_ON)
        {
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

                GameObject gameObjectGhost = GameObject.FindGameObjectWithTag(Globals.TAG_GHOST_BLUE);
                gameObjectGhost.SetActive(ghostBlueVisible);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                ghostOrangeVisible = !ghostOrangeVisible;

                GameObject gameObjectGhost = GameObject.FindGameObjectWithTag(Globals.TAG_GHOST_ORANGE);
                gameObjectGhost.SetActive(ghostOrangeVisible);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                ghostPinkVisible = !ghostPinkVisible;

                GameObject gameObjectGhost = GameObject.FindGameObjectWithTag(Globals.TAG_GHOST_PINK);
                gameObjectGhost.SetActive(ghostPinkVisible);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ghostRedVisible = !ghostRedVisible;

                GameObject gameObjectGhost = GameObject.FindGameObjectWithTag(Globals.TAG_GHOST_RED);
                gameObjectGhost.SetActive(ghostRedVisible);
            }
        }
    }

    private void updateIA()
    {
        //if (ghostBlueVisible) ghostBlueMove.onMove();
        if (ghostOrangeVisible) ghostOrangeMove.onMove(levelCreator.GetMap());
        //if (ghostPinkVisible) ghostPinkMove.onMove();
        //if (ghostRedVisible) ghostRedMove.onMove();
    }

    private void updateTimeBonus()
    {
        if (bonusPacmanKillsGhost)
        {
            if (bonusPacmanKillsGhostRemaining > 0)
            {
                if (bonusPacmanKillsGhostRemaining > 2)
                {

                }

                bonusPacmanKillsGhostRemaining -= Time.deltaTime;
            }
            else
            {
                ghostBlueMove.SetKilleable(false);
                ghostOrangeMove.SetKilleable(false);
                ghostPinkMove.SetKilleable(false);
                ghostRedMove.SetKilleable(false);
                bonusPacmanKillsGhost = false;
            }
        }
    }

    public void coinEaten()
    {
        // TODO Se deja asi por si se quiere implementar un multiplicador
        int multiplier = 1;
        increaseScore(COIN_SCORE * multiplier);
        --remainingCoins;

        if (remainingCoins == 0)
        {
            // Fin del juego
            if (currentLevel == TOTAL_LEVEL)
            {

            }
            else
            {
                //TODO Imagen final de nivel
                startMessage(GameGUI.TITLE_END_OF_LEVEL_TEXT, GameGUI.MESSAGE_END_OF_LEVEL_TEXT);
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

    public Vector3 getPacmanPosition()
    {
        return new Vector3(50.0f, 21.0f, 32.0f);
    }

    private void startMessage(string title, string message)
    {
        gameGUI.addMessageToQueue(title, message);
        gamePaused = true;
    }

    private void endMessage()
    {
        gameGUI.removeMessage();
        gamePaused = false;
    }

    public void setGamePaused(bool paused)
    {
        this.gamePaused = paused;
    }

    public bool getGamePaused()
    {
        return gamePaused;
    }

    public void bonusEaten()
    {
        bonusPacmanKillsGhost = true;
        setGhostsKilleables();
    }

    public bool isBonusPacmanKillsGhost()
    {
        return bonusPacmanKillsGhost;
    }

    public void setGhostsKilleables()
    {
        bonusPacmanKillsGhostRemaining = TIME_BONUS_PACMAN_KILLS_GHOST;
        ghostBlueMove.SetKilleable(true);
        ghostOrangeMove.SetKilleable(true);
        ghostPinkMove.SetKilleable(true);
        ghostRedMove.SetKilleable(true);
    }

    public void ghostEaten(string ghostTag)
    {
        if (ghostTag == Globals.TAG_GHOST_BLUE)
        {
            ghostBlueMove.SetDead(true);
        }
        else if (ghostTag == Globals.TAG_GHOST_ORANGE)
        {
            ghostOrangeMove.SetDead(true);
        }
        else if (ghostTag == Globals.TAG_GHOST_PINK)
        {
            ghostPinkMove.SetDead(true);
        }
        else if (ghostTag == Globals.TAG_GHOST_RED)
        {
            ghostRedMove.SetDead(true);
        }
    }
}
