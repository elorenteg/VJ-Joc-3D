using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuKeyboard : MonoBehaviour {

    private string[] mainMenuLabels = {"Jugar", "Instruccions", "Opcions", "Crèdits", "Sortir"};
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

        normalFont = new GUIStyle(); normalFont.fontSize = 28;
        selectFont = new GUIStyle(); selectFont.fontSize = 32;

        normalFont.alignment = TextAnchor.UpperCenter;
        selectFont.alignment = TextAnchor.UpperCenter;

        normalFont.normal.textColor = Color.yellow;
        selectFont.normal.textColor = new Color32(168,214,22,255);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.DownArrow) == true) {
            AudioSource.PlayClipAtPoint(moveSound, transform.position);

            mainMenuSelected++;
            mainMenuSelected = Mathf.Min(mainMenuSelected, mainMenuLabels.Length-1);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) == true) {
            AudioSource.PlayClipAtPoint(moveSound, transform.position);

            mainMenuSelected--;
            mainMenuSelected = Mathf.Max(mainMenuSelected, 0);
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
        float h_text = 40;
        float xo_text = width / 2 - w_text / 2;
        float yo_text = h1_space + h_logo + 50;

        float w_sep = 500;
        float h_sep = 60;

        float w_sel = 300;
        float h_sel = 60;
        float yo_sel = yo_text - h_text / 3;

        float inc_sep = (height/3)/mainMenuLabels.Length;

        Graphics.DrawTexture(new Rect(width / 2 - w_sep / 2, yo_text-h_sep, w_sep, h_sep), separatorTexture);
        Graphics.DrawTexture(new Rect(width / 2 - w_sep / 2, yo_text+mainMenuLabels.Length*inc_sep - 10, w_sep, h_sep), separatorTexture);
        for (int i = 0; i < mainMenuLabels.Length; i++) {
            if (mainMenuSelected == i) {
                GUI.DrawTexture(new Rect(width / 2 - w_sel / 2, yo_sel + i * inc_sep, w_sel, h_sel), selectTexture);
                GUI.Button(new Rect(xo_text, yo_text + i * inc_sep, w_text, h_text), mainMenuLabels[i], selectFont);
            }
            else GUI.Button(new Rect(xo_text, yo_text + i * inc_sep, w_text, h_text), mainMenuLabels[i], normalFont);
        }

        switch(mainMenuAction)
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
}