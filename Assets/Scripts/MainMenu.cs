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

    private Vector3 CAMERA_INIT_POS = new Vector3(7.0f, 0, -20.0f);
    private Vector3 PACMAN_INIT_POS = new Vector3(-24.0f, 6.5f, 35.0f);
    private Vector3 GHOST_INIT_POS = new Vector3(-36.0f, -8.0f, 49.0f);
    private Vector3 BONUS_INIT_POS = new Vector3(49.0f, -8.0f, 49.0f);

    private Vector3 PACMAN_DEST_POS = new Vector3(48.0f, 6.5f, 35.0f);
    private Vector3 GHOST_DEST_POS = new Vector3(38.0f, -8.0f, 49.0f);

    private Vector3 PACMAN_SCALE = new Vector3(6.0f, 6.0f, 6.0f);
    private Vector2 PACMAN_TEXTURE_SCALE = new Vector2(0.5f, 1.0f);
    private Vector3 GHOST_SCALE = new Vector3(5.0f, 5.0f, 5.0f);
    private Vector3 BONUS_SCALE = new Vector3(7.0f, 7.0f, 7.0f);

    private float PACMAN_SPEED = 15;
    private float GHOST_SPEED = 14;

    private Quaternion PACMAN_LOOKING_RIGHT = new Quaternion(0, 0, 0, 0);
    private Quaternion GHOST_LOOKING_RIGHT = new Quaternion(0, 0, 0, 0);
    private Quaternion PACMAN_LOOKING_LEFT = new Quaternion(0, 0, 0, 0);
    private Quaternion GHOST_LOOKING_LEFT = new Quaternion(0, 0, 0, 0);

    private enum State { Moving_to_bonus, Eating_bonus, Eating_ghost, Moving_to_base };

    private State currentPacmanState = State.Moving_to_bonus;

    public static int MAX_FRAMES_STATE = 15;
    private GhostAnimate ghostAnimateScript;
    private int textureState;
    private int frameState;
    private bool isDead;
    private bool canBeKilled;

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

        if (currentPacmanState == State.Moving_to_bonus)
        {
            movePacManToPoint(PACMAN_DEST_POS);
            moveGhostToPoint(GHOST_DEST_POS);
            rotatePacMan(PACMAN_LOOKING_RIGHT); //Ambos han de mirar hacia la derecha
            rotateGhost(GHOST_LOOKING_RIGHT);
        }
        else if (currentPacmanState == State.Eating_bonus)
        {
            eatingBonus();
            rotatePacMan(PACMAN_LOOKING_LEFT); //Ambos han de mirar hacia la izquierda
            rotateGhost(GHOST_LOOKING_LEFT);
        }
        else if (currentPacmanState == State.Eating_ghost)
        {
            eatingGhost();
            rotatePacMan(PACMAN_LOOKING_LEFT); //Ambos han de mirar hacia la izquierda
            rotateGhost(GHOST_LOOKING_LEFT);
        }
        else if (currentPacmanState == State.Moving_to_base)
        {
            moveGhostToPoint(GHOST_INIT_POS);
            movePacManToPoint(PACMAN_INIT_POS);
            rotatePacMan(PACMAN_LOOKING_LEFT); //Ambos han de mirar hacia la izquierda
            rotateGhost(GHOST_LOOKING_LEFT);
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

        /*float h_logo = Screen.height / 5;
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
        }*/
    }

    private void movePacManToPoint(Vector3 destination)
    {
        pacman.transform.position = Vector3.MoveTowards(pacman.transform.position, destination, PACMAN_SPEED * Time.deltaTime);

        if (pacman.transform.position == PACMAN_DEST_POS)
        {
            if (currentPacmanState == State.Eating_ghost)
            {
                currentPacmanState = State.Moving_to_bonus;
            }
            else
            {
                currentPacmanState = State.Eating_bonus;
            }
        }
        else if (pacman.transform.position == PACMAN_INIT_POS)
        {
            //Reinstanciar fantasma
            currentPacmanState = State.Moving_to_bonus;
            showBonus();
        }
    }

    private void moveGhostToPoint(Vector3 destination)
    {
        ghost.transform.position = Vector3.MoveTowards(ghost.transform.position, destination, GHOST_SPEED * Time.deltaTime);
    }

    private void showBonus()
    {
        bonus.transform.localScale = BONUS_SCALE;
    }

    private void hideBonus()
    {
        bonus.transform.localScale = new Vector3(0, 0, 0);
    }

    private void eatingBonus()
    {
        hideBonus();
        currentPacmanState = State.Eating_ghost;
    }

    private void eatingGhost()
    {
        //TODO Poner fantasma como comible
        Vector3 pacManDestination = new Vector3(ghost.transform.position.x, pacman.transform.position.y, pacman.transform.position.z);

        pacman.transform.position = Vector3.MoveTowards(pacman.transform.position, pacManDestination, PACMAN_SPEED * Time.deltaTime);
        ghost.transform.position = Vector3.MoveTowards(ghost.transform.position, GHOST_INIT_POS, GHOST_SPEED * Time.deltaTime);

        if (pacman.transform.position == ghost.transform.position)
        {
            killGhost();
        }
        currentPacmanState = State.Moving_to_base;
    }

    private void setGhostKilleable()
    {
        ghostAnimateScript.SetTextures(ghostAnimateScript.stateKilleable(), textureState);
    }

    private void killGhost()
    {
        Destroy(ghost);
    }

    private void rotatePacMan(Quaternion orientation)
    {

    }

    private void rotateGhost(Quaternion orientation)
    {

    }

    public void UpdateTextures()
    {
        if (isDead) ghostAnimateScript.SetTextures(ghostAnimateScript.stateDead(), textureState);
        else if (canBeKilled) ghostAnimateScript.SetTextures(ghostAnimateScript.stateKilleable(), textureState);
        else ghostAnimateScript.SetTextures(ghostAnimateScript.stateMove(), textureState);
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