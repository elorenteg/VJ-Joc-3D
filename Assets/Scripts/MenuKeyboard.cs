using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuKeyboard : MonoBehaviour {

    private string[] mainMenuLabels = {"Jugar", "Instruccions", "Opcions", "Crèdits", "Sortir"};
    //private bool[] mainMenuButtons;
    private const int JUGAR = 0;
    private const int INSTR = 1;
    private const int OPTNS = 2;
    private const int CREDS = 3;
    private const int SORTR = 4;

    private int mainMenuSelected;
    private int mainMenuAction;

    private GUIStyle normalFont;
    private GUIStyle selectFont;

    public Texture2D separatorTexture;
    public Texture2D selectTexture;
    public Texture2D backgroundTexture;
    public Texture2D logoTexture;

    public AudioClip moveSound;

    void Start() {
        mainMenuAction = -1;
        mainMenuSelected = 0;
        //mainMenuButtons = new bool[mainMenuLabels.Length];
        //mainMenuButtons[mainMenuSelected] = true;

        normalFont = new GUIStyle(); normalFont.fontSize = 28;
        selectFont = new GUIStyle(); selectFont.fontSize = 32;

        normalFont.alignment = TextAnchor.UpperCenter;
        selectFont.alignment = TextAnchor.UpperCenter;

        normalFont.normal.textColor = Color.yellow;
        selectFont.normal.textColor = new Color32(41,174,54,255);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.DownArrow) == true) {
            AudioSource.PlayClipAtPoint(moveSound, transform.position);
            //mainMenuButtons[mainMenuSelected] = false;

            mainMenuSelected++;
            mainMenuSelected = Mathf.Min(mainMenuSelected, mainMenuLabels.Length-1);
            
            //mainMenuButtons[mainMenuSelected] = true;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) == true) {
            AudioSource.PlayClipAtPoint(moveSound, transform.position);
            //mainMenuButtons[mainMenuSelected] = false;

            mainMenuSelected--;
            mainMenuSelected = Mathf.Max(mainMenuSelected, 0);
            
            //mainMenuButtons[mainMenuSelected] = true;
        }

        if (Input.GetKeyDown(KeyCode.Return) == true) {
            mainMenuAction = mainMenuSelected;
        }
    }

    void OnGUI() {
        float width = Screen.width;
        float height = Screen.height;
        Graphics.DrawTexture(new Rect(0, 0, width, height), backgroundTexture);

        float h1_space = height/6;

        float h_logo = height / 4;
        float w_logo = h_logo * 2.5f;
        Graphics.DrawTexture(new Rect(width / 2 - w_logo / 2, h1_space, w_logo, h_logo), logoTexture);

        float w_text = 200;
        float h_text = height/10;
        float xo_text = width / 2 - w_text / 2;
        float yo_text = h1_space + h_logo + 50;

        float w_sep = 500;
        float h_sep = height/15;

        float w_sel = 250;
        float h_sel = height/15;

        float inc_sep = (height/3)/mainMenuLabels.Length;

        Graphics.DrawTexture(new Rect(width / 2 - w_sep / 2, yo_text-h_sep, w_sep, h_sep), separatorTexture);
        Graphics.DrawTexture(new Rect(width / 2 - w_sep / 2, yo_text+mainMenuLabels.Length*inc_sep - 10, w_sep, h_sep), separatorTexture);
        for (int i = 0; i < mainMenuLabels.Length; i++) {
            if (mainMenuSelected == i) {
                GUI.Button(new Rect(xo_text, yo_text + i * inc_sep, w_text, h_text), mainMenuLabels[i], selectFont);
                Graphics.DrawTexture(new Rect(width / 2 - w_sel / 2, yo_text - h_sel/6 + i * inc_sep, w_sel, h_sel), selectTexture);
            }
            else GUI.Button(new Rect(xo_text, yo_text + i * inc_sep, w_text, h_text), mainMenuLabels[i], normalFont);
        }

        switch(mainMenuAction)
        {
            case JUGAR:
                SceneManager.LoadScene("Level2");
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
}