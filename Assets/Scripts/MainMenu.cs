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

    private Vector3 PACMAN_SCALE = new Vector3(6.0f, 6.0f, 6.0f);
    private Vector2 PACMAN_TEXTURE_SCALE = new Vector2(0.5f, 1.0f);
    private Vector3 GHOST_SCALE = new Vector3(1.0f, 1.0f, 1.0f);

    void Start()
    {
        mainMenuAction = -1;
        mainMenuSelected = 0;

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

        SetInitCameraPosition(0, 0, 0);

        instantiatePacMan();
        instantiateGhosts();
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

    private void instantiatePacMan()
    {
        GameObject element = pacman;
        Vector3 position = new Vector3(0, 0, 0);
        Vector3 scale = new Vector3(PACMAN_SCALE.x, PACMAN_SCALE.y, PACMAN_SCALE.z);

        GameObject newObject = Instantiate(element, position, element.transform.rotation) as GameObject;
        newObject.transform.parent = transform;
        newObject.transform.localScale = scale;
    }

    private void instantiateGhosts()
    {
        
    }


    private void SetInitCameraPosition(int x, int y, int z)
    {
        Vector3 cameraPosition = new Vector3(x, y, z);

        m_camera.transform.position = cameraPosition;
    }
}