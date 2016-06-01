using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    private string[] mainMenuLabels = { "play", "how to", "options", "credits", "exit" };
    private const int JUGAR = 0;
    private const int INSTR = 1;
    private const int OPTNS = 2;
    private const int CREDS = 3;
    private const int SORTR = 4;

    private int mainMenuSelected;
    private int mainMenuAction;

    private GUIStyle normalFont;
    private GUIStyle selectFont;

    public Texture2D selectTexture;
    public Texture2D backgroundTexture;

    //http://gameswallpaperhd.com/request-making-a-bioshock-infinite-monopoly-help-bioshock.html
    public Texture2D logoTexture;
    public Texture2D logoBioTexture;

    public AudioClip moveSound;

    public Camera m_camera;
    public GameObject pacman;
    public GameObject ghost;
    public Texture blueGhostTexture;
    public Texture orangeGhostTexture;
    public Texture pinkGhostTexture;
    public Texture redGhostTexture;
    public GameObject bonus;

    private Vector3 CAMERA_INIT_POS = new Vector3(7.0f, -3.0f, 10.0f);
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
        mainMenuAction = -1;
        mainMenuSelected = 0;
        textureState = 0;
        frameState = 0;

        normalFont = new GUIStyle();
        normalFont.fontSize = 24;
        normalFont.alignment = TextAnchor.UpperCenter;
        normalFont.normal.textColor = new Color32(119, 136, 153, 255);
        normalFont.font = (Font)Resources.Load("Fonts/namco", typeof(Font));

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
        if (Input.GetKeyDown(KeyCode.DownArrow) == true)
        {
            AudioSource.PlayClipAtPoint(moveSound, transform.position);

            mainMenuSelected++;
            mainMenuSelected = Mathf.Min(mainMenuSelected, mainMenuLabels.Length - 1);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) == true)
        {
            AudioSource.PlayClipAtPoint(moveSound, transform.position);

            mainMenuSelected--;
            mainMenuSelected = Mathf.Max(mainMenuSelected, 0);
        }

        if (Input.GetKeyDown(KeyCode.Return) == true)
        {
            mainMenuAction = mainMenuSelected;
        }

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

    void OnGUI()
    {
        //Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);

        float h_logo = Screen.height / 5;
        float w_logo = h_logo * 3.75f;
        Graphics.DrawTexture(new Rect(Screen.width / 2 - w_logo / 2, Screen.height / 9, w_logo, h_logo), logoTexture);

        float h_bio_logo = Screen.height / 4;
        float w_bio_logo = h_logo * 1.5f;
        Graphics.DrawTexture(new Rect(Screen.width * 0.82f - w_bio_logo / 2, 0, w_bio_logo, h_bio_logo), logoBioTexture);

        float w_text = 200;
        float h_text = 40;
        float xo_text = Screen.width / 14;
        float yo_text = Screen.height / 5 + h_logo;

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

        switch (mainMenuAction)
        {
            case JUGAR:
                SceneManager.LoadScene("Level1");
                break;
            case INSTR:
                break;
            case OPTNS:
                break;
            case CREDS:
                break;
            case SORTR:
                Application.Quit();
                break;
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
        } else if (currentPacmanState == State.Eating_ghost)
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
        switch(orientation)
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