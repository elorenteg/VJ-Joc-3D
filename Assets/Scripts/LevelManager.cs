using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{

    private static int TOTAL_LEVEL = 3;
    private static int INITIAL_LEVEL = 1;
    private static int INITIAL_LIFES = 3;
    private static int COIN_SCORE = 1;
    private static int GHOST_SCORE = 100;
    private int currentGhostScore;

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
    private static int currentMessage;
    private const int NO_MSS = 0;
    private const int END_OF_LEVEL_MSS = 1;
    private const int END_OF_GAME_MSS = 2;
    private const int LOST_LIFE_MSS = 3;
    private const int GAME_OVER_MSS = 4;
    private const int PAUSE_MSS = 5;

    private static int TIME_BONUS_PACMAN_KILLS_GHOST = 10; //5 seconds
    private bool bonusPacmanKillsGhost;
    private float bonusPacmanKillsGhostRemaining;

    private static int TIME_BONUS_SHOWING_CHERRY = 5; //5 seconds
    private static int TIME_BONUS_HIDING_CHERRY = 10; //10 seconds
    private bool bonusCherryShowing;
    private float bonusCherryShowingRemaining;
    private float bonusCherryHidingRemaining;

    private bool showScoreBGhost;
    private bool showScoreOGhost;
    private bool showScorePGhost;
    private bool showScoreRGhost;
    private bool showScoreCherry;
    private int scoreBGhost;
    private int scoreOGhost;
    private int scoreRGhost;
    private int scorePGhost;
    private int scoreCherry;
    private Vector3 scoreBGhostPosition;
    private Vector3 scoreOGhostPosition;
    private Vector3 scorePGhostPosition;
    private Vector3 scoreRGhostPosition;
    private Vector3 scoreCherryPosition;

    private int MAX_TIME_SHOW_SCORE = 5;
    private float timeScoreBGhost;
    private float timeScoreOGhost;
    private float timeScorePGhost;
    private float timeScoreRGhost;
    private float timeScoreCherry;

    private AudioSource audioSource;
    public AudioClip bonusStateAudio;
    public float bonusStateVolume;

    void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        levelCreator = gameManager.GetComponent<LevelCreator>();
        dataManager = gameManager.GetComponent<DataManager>();
        gameGUI = gameManager.GetComponent<GameGUI>();

        audioSource = GetComponent<AudioSource>();

        currentHighScore = dataManager.readMaxScore();
        gamePaused = false;
        bonusPacmanKillsGhost = false;
        bonusPacmanKillsGhostRemaining = 0.0f;
        bonusCherryShowingRemaining = TIME_BONUS_SHOWING_CHERRY;
        bonusCherryHidingRemaining = 0;
        bonusCherryShowing = true;
        currentGhostScore = GHOST_SCORE;

        startGame();
    }

    void startGame()
    {
        currentLevel = INITIAL_LEVEL;
        currentMessage = NO_MSS;
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

        remainingCoins = LevelCreator.NUM_COINS_LEVEL;

        ghostBlueVisible = true;
        ghostOrangeVisible = true;
        ghostPinkVisible = true;
        ghostRedVisible = true;

        showScoreBGhost = false;
        showScoreOGhost = false;
        showScorePGhost = false;
        showScoreRGhost = false;
        showScoreCherry = false;
    }

    void OnGUI()
    {
        updateShowScores();
    }

    void Update()
    {
        if (!gamePaused)
        {
            if ((Input.GetKeyDown(KeyCode.Escape) == true || Input.GetKeyDown(KeyCode.P) == true) && currentMessage == NO_MSS)
            {
                currentMessage = PAUSE_MSS;
                startMessage(GameGUI.TITLE_PAUSE_TEXT, GameGUI.MESSAGE_PAUSE_TEXT, GameGUI.TIME_BEFORE_MESSAGE_PAUSE);
            }

            updateIA();
            updateTimeBonus();
            updateTimeCherry();
        }
        else
        {
            if (Input.GetKey(KeyCode.Return))
            {
                switch (currentMessage)
                {
                    case END_OF_LEVEL_MSS:
                        if (remainingCoins == 0)
                        {
                            ++currentLevel;
                            loadLevel(currentLevel);
                        }
                        break;
                    case END_OF_GAME_MSS:
                        startGame();
                        break;
                    case LOST_LIFE_MSS:
                        loadLevel(currentLevel);
                        break;
                    case GAME_OVER_MSS:
                        startGame();
                        break;
                }

                endMessage();
            }
            if (currentMessage == PAUSE_MSS)
            {
                if (Input.GetKeyDown(KeyCode.Return) == true)
                {
                    endMessage();
                }
                else if (Input.GetKeyDown(KeyCode.Escape) == true)
                {
                    Application.Quit();
                }
            }
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

            GhostMove moveScript;
            if (Input.GetKeyDown(KeyCode.B))
            {
                ghostBlueVisible = !ghostBlueVisible;

                GameObject gameObjectGhost = GameObject.FindGameObjectWithTag(Globals.TAG_GHOST_BLUE);
                moveScript = gameObjectGhost.GetComponent<GhostBlueMove>();
                moveScript.SetVisible(ghostBlueVisible);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                ghostOrangeVisible = !ghostOrangeVisible;

                GameObject gameObjectGhost = GameObject.FindGameObjectWithTag(Globals.TAG_GHOST_ORANGE);
                moveScript = gameObjectGhost.GetComponent<GhostOrangeMove>();
                moveScript.SetVisible(ghostOrangeVisible);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                ghostPinkVisible = !ghostPinkVisible;

                GameObject gameObjectGhost = GameObject.FindGameObjectWithTag(Globals.TAG_GHOST_PINK);
                moveScript = gameObjectGhost.GetComponent<GhostPinkMove>();
                moveScript.SetVisible(ghostPinkVisible);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ghostRedVisible = !ghostRedVisible;

                GameObject gameObjectGhost = GameObject.FindGameObjectWithTag(Globals.TAG_GHOST_RED);
                moveScript = gameObjectGhost.GetComponent<GhostRedMove>();
                moveScript.SetVisible(ghostRedVisible);
            }
        }
    }

    private void updateIA()
    {
        if (ghostBlueVisible) ghostBlueMove.onMove(levelCreator.GetMap());
        if (ghostOrangeVisible) ghostOrangeMove.onMove(levelCreator.GetMap());
        if (ghostPinkVisible) ghostPinkMove.onMove(levelCreator.GetMap());
        if (ghostRedVisible) ghostRedMove.onMove(levelCreator.GetMap());
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
                
                if (!audioSource.isPlaying)
                {
                    audioSource.volume = bonusStateVolume;
                    audioSource.clip = bonusStateAudio;
                    audioSource.Play();
                }
            }
            else
            {
                ghostBlueMove.SetKilleable(false);
                ghostOrangeMove.SetKilleable(false);
                ghostPinkMove.SetKilleable(false);
                ghostRedMove.SetKilleable(false);
                bonusPacmanKillsGhost = false;
                currentGhostScore = GHOST_SCORE;
            }
        }
    }

    private void updateTimeCherry()
    {
        if (bonusCherryShowing)
        {
            if (bonusCherryShowingRemaining > 0)
            {
                bonusCherryShowingRemaining -= Time.deltaTime;
            }
            else
            {
                levelCreator.destroyObject(Globals.TAG_CHERRY);
                bonusCherryHidingRemaining = TIME_BONUS_HIDING_CHERRY;
                bonusCherryShowing = false;
            }
        }
        else
        {
            if (bonusCherryHidingRemaining > 0)
            {
                bonusCherryHidingRemaining -= Time.deltaTime;
            }
            else
            {
                levelCreator.instantiateCherry();
                bonusCherryShowingRemaining = TIME_BONUS_SHOWING_CHERRY;
                bonusCherryShowing = true;
            }
        }
    }

    public void updateShowScores()
    {
        GUIStyle infoFont = new GUIStyle();
        infoFont.font = (Font)Resources.Load("Fonts/lilliput steps", typeof(Font));
        infoFont.fontSize = 28;
        infoFont.alignment = TextAnchor.UpperRight;
        infoFont.normal.textColor = Color.green;

        if (showScoreBGhost)
        {
            timeScoreBGhost -= Time.deltaTime;
            if (timeScoreBGhost <= 0) showScoreBGhost = false;
            else GUI.Label(new Rect(scoreBGhostPosition.x - 150, scoreBGhostPosition.y - 50, 300, 100), 
                scoreBGhost.ToString(), infoFont);
        }

        if (showScoreOGhost)
        {
            timeScoreOGhost -= Time.deltaTime;
            if (timeScoreOGhost <= 0) showScoreOGhost = false;
            else GUI.Label(new Rect(scoreOGhostPosition.x - 150, scoreOGhostPosition.y - 50, 300, 100),
                scoreOGhost.ToString(), infoFont);
        }

        if (showScorePGhost)
        {
            timeScorePGhost -= Time.deltaTime;
            if (timeScorePGhost <= 0) showScorePGhost = false;
            else GUI.Label(new Rect(scorePGhostPosition.x - 150, scorePGhostPosition.y - 50, 300, 100),
                scorePGhost.ToString(), infoFont);
        }

        if (showScoreRGhost)
        {
            timeScoreRGhost -= Time.deltaTime;
            if (timeScoreRGhost <= 0) showScoreRGhost = false;
            else GUI.Label(new Rect(scoreRGhostPosition.x - 150, scoreRGhostPosition.y - 50, 300, 100),
                scoreRGhost.ToString(), infoFont);
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
                startMessage(GameGUI.TITLE_END_OF_GAME_TEXT, GameGUI.MESSAGE_END_OF_GAME_TEXT, GameGUI.TIME_BEFORE_MESSAGE_END_OF_LEVEL);
                currentMessage = END_OF_GAME_MSS;
            }
            else
            {
                //TODO Imagen final de nivel
                startMessage(GameGUI.TITLE_END_OF_LEVEL_TEXT, GameGUI.MESSAGE_END_OF_LEVEL_TEXT, GameGUI.TIME_BEFORE_MESSAGE_END_OF_LEVEL);
                currentMessage = END_OF_LEVEL_MSS;
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

        if (currentLifes == 0)
        {
            startMessage(GameGUI.TITLE_GAME_OVER_TEXT, GameGUI.MESSAGE_GAME_OVER_TEXT, GameGUI.TIME_BEFORE_MESSAGE_LOST_LIFE);
            currentMessage = GAME_OVER_MSS;
        }
        else
        {
            startMessage(GameGUI.TITLE_LOST_LIFE_TEXT, GameGUI.MESSAGE_LOST_LIFE_TEXT, GameGUI.TIME_BEFORE_MESSAGE_LOST_LIFE);
            currentMessage = LOST_LIFE_MSS;
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

    private void startMessage(string title, string message, int timeBeforeShow)
    {
        gameGUI.queueMessage(title, message, timeBeforeShow);
        gamePaused = true;
    }

    private void endMessage()
    {
        gameGUI.removeMessage();
        gamePaused = false;

        currentMessage = NO_MSS;
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

    public void ghostEaten(string ghostTag, Vector3 pos)
    {
        increaseScore(currentGhostScore);

        if (ghostTag == Globals.TAG_GHOST_BLUE)
        {
            ghostBlueMove.SetDead(true);
            showScoreBGhost = true;
            timeScoreBGhost = MAX_TIME_SHOW_SCORE;
            scoreBGhost = currentGhostScore;
            scoreBGhostPosition = Camera.main.WorldToScreenPoint(pos);
            scoreBGhostPosition.y = Screen.height - scoreBGhostPosition.y;
        }
        else if (ghostTag == Globals.TAG_GHOST_ORANGE)
        {
            ghostOrangeMove.SetDead(true);
            showScoreOGhost = true;
            timeScoreOGhost = MAX_TIME_SHOW_SCORE;
            scoreOGhost = currentGhostScore;
            scoreOGhostPosition = Camera.main.WorldToScreenPoint(pos);
            scoreOGhostPosition.y = Screen.height - scoreOGhostPosition.y;
        }
        else if (ghostTag == Globals.TAG_GHOST_PINK)
        {
            ghostPinkMove.SetDead(true);
            showScorePGhost = true;
            timeScorePGhost = MAX_TIME_SHOW_SCORE;
            scorePGhost = currentGhostScore;
            scorePGhostPosition = Camera.main.WorldToScreenPoint(pos);
            scorePGhostPosition.y = Screen.height - scorePGhostPosition.y;
        }
        else if (ghostTag == Globals.TAG_GHOST_RED)
        {
            ghostRedMove.SetDead(true);
            showScoreRGhost = true;
            timeScoreRGhost = MAX_TIME_SHOW_SCORE;
            scoreRGhost = currentGhostScore;
            scoreRGhostPosition = Camera.main.WorldToScreenPoint(pos);
            scoreRGhostPosition.y = Screen.height - scoreRGhostPosition.y;
        }
        currentGhostScore += GHOST_SCORE;
    }

    public int[][] GetMap()
    {
        return levelCreator.GetMap();
    }

    public bool lastTimeBonus()
    {
        if (bonusPacmanKillsGhostRemaining <= TIME_BONUS_PACMAN_KILLS_GHOST / 3) return true;
        else return false;
    }
}
