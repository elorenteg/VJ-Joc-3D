using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    private string[] mainMenuLabels = { "play", "how to", "options", "credits", "exit" };
    private const int MENU = -1;
    private const int JUGAR = 0;
    private const int INSTR = 1;
    private const int OPTNS = 2;
    private const int CREDS = 3;
    private const int SORTR = 4;

    private const string MENU_INSTRUCTIONS_TITLE = "how to play";
    private const string MENU_INSTRUCTIONS = @"
        use the keyboard arrows to move the pacman.
        (or use the wasd if you are a fps player.)

        take all the coins to win the game.
        you can also get the hammers and kill ghosts, 
        just for fun.

        take care because four ghosts will try to eat you,
        if you die you will have two more lifes... but...
        if you lose all your lives you will have to 
        start all the game again!";

    private int mainMenuSelected;
    private int mainMenuAction;

    private GUIStyle normalFont;
    private GUIStyle menu_titleFont;
    private GUIStyle menu_descriptionFont;
    private GUIStyle selectFont;

    public Texture2D selectTexture;
    public Texture2D backgroundTexture;
    public Texture2D messageTexture;

    public AudioClip moveSound;

    public Camera m_camera;
    public GameObject pacman;
    public GameObject ghost;
    public Texture blueGhostTexture;
    public Texture orangeGhostTexture;
    public Texture pinkGhostTexture;
    public Texture redGhostTexture;
    public GameObject bonus;

    private Vector3 CAMERA_INIT_POS = new Vector3(7.0f, 7.0f, 10.0f);
    private Vector3 PACMAN_INIT_POS = new Vector3(-4.0f, 6.5f, 35.0f);
    private Vector3 GHOST_INIT_POS = new Vector3(-16.0f, -8.0f, 49.0f);
    private Vector3 BONUS_INIT_POS = new Vector3(40.0f, -8.0f, 49.0f);

    private Vector3 PACMAN_DEST_POS = new Vector3(36.0f, 6.5f, 35.0f);
    private Vector3 GHOST_DEST_POS = new Vector3(26.0f, -8.0f, 49.0f);

    private Vector3 PACMAN_SCALE = new Vector3(6.0f, 6.0f, 6.0f);
    private Vector2 PACMAN_TEXTURE_SCALE = new Vector2(0.5f, 1.0f);
    private Vector3 GHOST_SCALE = new Vector3(5.0f, 5.0f, 5.0f);
    private Vector3 BONUS_SCALE = new Vector3(7.0f, 7.0f, 7.0f);

    private int PACMAN_SPEED = 13;
    private float GHOST_SPEED_ALIVE = 13;
    private float GHOST_SPEED_KILLEABLE = 8.5f;
    private float GHOST_SPEED_DEAD = 16.5f;

    private const int PACMAN_LOOKING_RIGHT = 0;
    private const int GHOST_LOOKING_RIGHT = 1;
    private const int PACMAN_LOOKING_LEFT = 2;
    private const int GHOST_LOOKING_LEFT = 3;

    private enum State { Moving_to_bonus, Eating_bonus, Eating_ghost, Moving_to_base };
    private enum StateGhost { Alive, Dead, Killeable };

    private State currentPacmanState = State.Moving_to_bonus;
    private StateGhost currentGhostState = StateGhost.Alive;

    private float GHOST_AUDIO_VOLUME = 0.5f;

    private PacmanAnimate pacmanAnimateScript;
    private GhostAnimate ghostAnimateScript;

    public static int MAX_FRAMES_STATE = 15;
    private int textureState;
    private int frameState;

    void Start()
    {
        mainMenuAction = MENU;
        mainMenuSelected = 0;
        textureState = 0;
        frameState = 0;

        normalFont = new GUIStyle();
        normalFont.fontSize = 24;
        normalFont.alignment = TextAnchor.UpperCenter;
        normalFont.normal.textColor = new Color32(119, 136, 153, 255);
        normalFont.font = (Font)Resources.Load("Fonts/namco", typeof(Font));

        menu_titleFont = new GUIStyle();
        menu_titleFont.fontSize = 40;
        menu_titleFont.alignment = TextAnchor.UpperCenter;
        menu_titleFont.normal.textColor = new Color32(255, 255, 255, 255);
        menu_titleFont.font = (Font)Resources.Load("Fonts/karma future", typeof(Font));

        menu_descriptionFont = new GUIStyle();
        menu_descriptionFont.fontSize = 20;
        menu_descriptionFont.alignment = TextAnchor.UpperCenter;
        menu_descriptionFont.normal.textColor = new Color32(255, 255, 255, 255);
        menu_descriptionFont.font = (Font)Resources.Load("Fonts/failed attempt", typeof(Font));

        selectFont = new GUIStyle();
        selectFont.fontSize = 28;
        selectFont.alignment = TextAnchor.UpperCenter;
        selectFont.normal.textColor = new Color32(47, 79, 79, 255);
        selectFont.font = (Font)Resources.Load("Fonts/namco", typeof(Font));

        SetInitCameraPosition(CAMERA_INIT_POS);

        instantiatePacMan();
        instantiateGhosts();
        instantiateBonus();
    }

    void Update()
    {
        if (mainMenuAction == MENU)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) == true)
            {
                AudioSource.PlayClipAtPoint(moveSound, transform.position);

                mainMenuSelected++;
                mainMenuSelected = Mathf.Min(mainMenuSelected, mainMenuLabels.Length - 1);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) == true)
            {
                AudioSource.PlayClipAtPoint(moveSound, transform.position);

                mainMenuSelected--;
                mainMenuSelected = Mathf.Max(mainMenuSelected, 0);
            }
            else if (Input.GetKeyDown(KeyCode.Return) == true)
            {
                mainMenuAction = mainMenuSelected;
            }
        }
        else if (mainMenuAction == INSTR)
        {
            if (Input.GetKeyDown(KeyCode.Escape) == true)
            {
                mainMenuAction = MENU;
            }
        }
        else if (mainMenuAction == OPTNS)
        {
            if (Input.GetKeyDown(KeyCode.Escape) == true)
            {
                mainMenuAction = MENU;
            }
        }
        else if (mainMenuAction == CREDS)
        {
            if (Input.GetKeyDown(KeyCode.Escape) == true)
            {
                mainMenuAction = MENU;
            }
        }

        if (mainMenuAction != MENU)
        {

            if (ghost.transform.position == GHOST_DEST_POS && currentPacmanState == State.Moving_to_bonus)
            {
                currentPacmanState = State.Eating_bonus;
                ghostAnimateScript.rotateBounds(-180);
                pacmanAnimateScript.rotateBounds(-180);
            }
            else if (ghost.transform.position == GHOST_DEST_POS && currentPacmanState == State.Eating_bonus)
            {
                currentPacmanState = State.Eating_ghost;
                ghostAnimateScript.rotateBounds(-180);
                pacmanAnimateScript.rotateBounds(-180);
            }
            else if (ghost.transform.position == GHOST_INIT_POS)
            {
                currentPacmanState = State.Moving_to_bonus;
                showBonus();
                showGhost(GHOST_INIT_POS);
                ghostAnimateScript.rotateBounds(180);
                pacmanAnimateScript.rotateBounds(180);
            }

            if (currentPacmanState == State.Moving_to_bonus)
            {
                currentGhostState = StateGhost.Alive;
                movingToBonus();
            }
            else if (currentPacmanState == State.Eating_bonus)
            {
                currentGhostState = StateGhost.Killeable;
                eatingBonus();
            }
            else if (currentPacmanState == State.Eating_ghost)
            {
                eatingGhost();
                movingToBase();
            }
            else if (currentPacmanState == State.Moving_to_base)
            {
                currentGhostState = StateGhost.Dead;
                movingToBase();
            }

            if (frameState == MAX_FRAMES_STATE)
            {
                frameState = 0;
                textureState = (textureState + 1) % 2;

                UpdateTextures();
            }
            ++frameState;
        }
    }

    void OnGUI()
    {
        switch (mainMenuAction)
        {
            case MENU:
                showMenu();
                break;
            case JUGAR:
                SceneManager.LoadScene("Level1");
                break;
            case INSTR:
                showInstructions();
                break;
            case OPTNS:
                showOptions();
                break;
            case CREDS:
                showCredits();
                break;
            case SORTR:
                Application.Quit();
                break;
        }
    }

    private void showInstructions()
    {
        float widthmessage = Screen.width / 1.5f;
        float heightmessage = Screen.height / 1.5f;
        float positionmessageX = Screen.width / 2f - widthmessage / 2;
        float positionmessageY = Screen.height / 20;

        float widthtitle = Screen.width / 3.8f;
        float heighttitle = Screen.height / 8.5f;
        float positiontitleX = positionmessageX + widthmessage / 2 - widthtitle / 2;
        float positiontitleY = positionmessageY - 20f;

        GUI.DrawTexture(new Rect(positionmessageX, positionmessageY, widthmessage, heightmessage), messageTexture);

        GUI.DrawTexture(new Rect(positiontitleX, positiontitleY, widthtitle, heighttitle), messageTexture);

        GUI.Label(new Rect(positiontitleX + widthtitle / 2, positiontitleY, 0, 0), MENU_INSTRUCTIONS_TITLE, menu_titleFont);

        GUI.Label(new Rect(positionmessageX + widthmessage / 2 - 10.0f, Screen.height / 9.5f, 0, 0), MENU_INSTRUCTIONS, menu_descriptionFont);
    }

    private void showOptions()
    {
        int positionmessageX = Screen.width / 4;
        int positionmessageY = Screen.height / 4;
        int widthmessage = Screen.width / 2;
        int heightmessage = Screen.height / 2;

        GUI.DrawTexture(new Rect(positionmessageX, positionmessageY, widthmessage, heightmessage), messageTexture);
    }

    private void showCredits()
    {
        int positionmessageX = Screen.width / 4;
        int positionmessageY = Screen.height / 4;
        int widthmessage = Screen.width / 2;
        int heightmessage = Screen.height / 2;

        GUI.DrawTexture(new Rect(positionmessageX, positionmessageY, widthmessage, heightmessage), messageTexture);
    }

    private void showMenu()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);

        float w_text = 200;
        float h_text = 40;
        float xo_text = Screen.width / 14;
        float yo_text = Screen.height / 2;

        //float w_sel = 250;
        //float h_sel = 90;
        float yo_sel = yo_text - h_text / 2;

        float inc_sep = (Screen.height / 2.25f) / mainMenuLabels.Length;

        for (int i = 0; i < mainMenuLabels.Length; i++)
        {
            if (mainMenuSelected == i)
            {
                //GUI.DrawTexture(new Rect(Screen.width / 4.5f - w_sel / 2, yo_sel + i * inc_sep, w_sel, h_sel), selectTexture);
                GUI.Label(new Rect(xo_text, yo_text + i * inc_sep, w_text, h_text), mainMenuLabels[i], selectFont);
            }
            else GUI.Label(new Rect(xo_text, yo_text + i * inc_sep, w_text, h_text), mainMenuLabels[i], normalFont);
        }
    }

    private void movingToBonus()
    {
        movePacManToPoint(PACMAN_LOOKING_RIGHT, PACMAN_SPEED);
        moveGhostToPoint(GHOST_DEST_POS, GHOST_SPEED_ALIVE);

        rotatePacMan(PACMAN_LOOKING_RIGHT); //Ambos han de mirar hacia la derecha
        rotateGhost(GHOST_LOOKING_RIGHT);
    }

    private void movingToBase()
    {
        movePacManToPoint(PACMAN_LOOKING_LEFT, PACMAN_SPEED);
        if (currentGhostState == StateGhost.Dead)
            moveGhostToPoint(GHOST_INIT_POS, GHOST_SPEED_DEAD);
        else
            moveGhostToPoint(GHOST_INIT_POS, GHOST_SPEED_KILLEABLE);


        rotatePacMan(PACMAN_LOOKING_LEFT); //Ambos han de mirar hacia la izquierda
        rotateGhost(GHOST_LOOKING_LEFT);
    }

    private void eatingBonus()
    {
        hideBonus();
        currentPacmanState = State.Eating_ghost;
    }

    private void eatingGhost()
    {
        setGhostKilleable();

        if (pacman.transform.position.x - 4 < ghost.transform.position.x)
        {
            currentPacmanState = State.Moving_to_base;
            hideGhost();
        }
    }

    private void setGhostKilleable()
    {
        ghostAnimateScript.SetTextures(ghostAnimateScript.stateKilleable(), textureState);
    }

    private void showGhost(Vector3 position)
    {
        ghost.transform.position = position;
        ghost.transform.localScale = GHOST_SCALE;
    }

    private void hideGhost()
    {
        //ghost.transform.localScale = new Vector3(0, 0, 0);
        ghostAnimateScript.StopSound();
        ghostAnimateScript.PlaySound(ghostAnimateScript.stateDead(), GHOST_AUDIO_VOLUME);
        //ghostAnimateScript.Animate(ghostAnimateScript.stateDead());
    }

    private void movePacManToPoint(int direction, int speed)
    {
        if (direction == PACMAN_LOOKING_LEFT)
            pacman.transform.Translate(-speed * Time.deltaTime, 0, 0);
        else
            pacman.transform.Translate(-speed * Time.deltaTime, 0, 0);

        //pacman.transform.position = Vector3.MoveTowards(pacman.transform.position, destination, speed * Time.deltaTime);
        pacmanAnimateScript.Animate(pacmanAnimateScript.stateMove());
        pacmanAnimateScript.PlaySound(pacmanAnimateScript.stateMove());
    }

    private void moveGhostToPoint(Vector3 destination, float speed)
    {
        ghost.transform.position = Vector3.MoveTowards(ghost.transform.position, destination, speed * Time.deltaTime);
        if (currentPacmanState == State.Moving_to_bonus)
        {
            ghostAnimateScript.PlaySound(ghostAnimateScript.stateMove(), GHOST_AUDIO_VOLUME);
        }
        else if (currentPacmanState == State.Eating_ghost)
        {
            ghostAnimateScript.PlaySound(ghostAnimateScript.stateKilleable(), GHOST_AUDIO_VOLUME);
        }
    }

    private void showBonus()
    {
        bonus.transform.localScale = BONUS_SCALE;
    }

    private void hideBonus()
    {
        bonus.transform.localScale = new Vector3(0, 0, 0);
    }

    private void rotatePacMan(int orientation)
    {
        //pacmanAnimateScript.rotateBounds(-180);
    }

    private void rotateGhost(int orientation)
    {
        // esto NO se ha de hacer cada frame
        switch (orientation)
        {
            case GHOST_LOOKING_LEFT:
                // ghostAnimateScript.rotateBounds(-180);
                break;
            case GHOST_LOOKING_RIGHT:
                // ghostAnimateScript.rotateBounds(180);
                break;
        }
    }

    public void UpdateTextures()
    {
        pacmanAnimateScript.SetTextures(textureState);

        if (currentGhostState == StateGhost.Killeable) ghostAnimateScript.SetTextures(ghostAnimateScript.stateKilleable(), textureState);
        else if (currentGhostState == StateGhost.Alive) ghostAnimateScript.SetTextures(ghostAnimateScript.stateMove(), textureState);
        else ghostAnimateScript.SetTextures(ghostAnimateScript.stateDead(), textureState);
    }

    private void instantiatePacMan()
    {
        GameObject element = pacman;
        Vector3 position = PACMAN_INIT_POS;
        Vector3 scale = PACMAN_SCALE;

        GameObject newObject = Instantiate(element, position, element.transform.rotation) as GameObject;
        newObject.transform.parent = transform;
        newObject.transform.localScale = scale;

        pacman = newObject;

        pacmanAnimateScript = newObject.GetComponent<PacmanAnimate>();
        pacmanAnimateScript.Start();
        pacmanAnimateScript.SetTextures(pacmanAnimateScript.stateMove());
        //pacmanAnimateScript.rotateBounds(180);
    }

    private void instantiateGhosts()
    {
        GameObject element = ghost;
        Vector3 position = GHOST_INIT_POS;
        Vector3 scale = GHOST_SCALE;

        GameObject newObject = Instantiate(element, position, element.transform.rotation) as GameObject;
        newObject.transform.parent = transform;
        newObject.transform.localScale = scale;

        ghost = newObject;

        ghostAnimateScript = newObject.GetComponent<GhostAnimate>();
        ghostAnimateScript.Start();
        ghostAnimateScript.SetTextures(ghostAnimateScript.stateMove(), textureState);
        //ghostAnimateScript.rotateBounds(180);
    }

    private void instantiateBonus()
    {
        GameObject element = bonus;
        Vector3 position = BONUS_INIT_POS;
        Vector3 scale = BONUS_SCALE;

        GameObject newObject = Instantiate(element, position, element.transform.rotation) as GameObject;
        newObject.transform.parent = transform;
        newObject.transform.localScale = scale;

        bonus = newObject;
    }

    private void SetInitCameraPosition(Vector3 position)
    {
        m_camera.transform.position = position;
    }
}